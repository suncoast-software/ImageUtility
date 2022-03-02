using ImageUtility.Interfaces;
using ImageUtility.Utility.Commands;
using Ookii.Dialogs.Wpf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageUtility.ViewModels
{
    internal class ResizeViewModel: BaseViewModel
    {
        private IMessageService _messageService;
        private ProgressDialog _progressDialog;
        public ICommand SourceDirCommand { get; set; }
        public ICommand TargetDirCommand { get; set;}
        public ICommand ResizeCommand { get; set;}
        public ICommand ClearInputCommand { get; set; }
        public ICommand CloseErrorMessageCommand { get; set; }

        private ObservableCollection<string> _imgList;
        public ObservableCollection<string> ImgList
        {
            get => _imgList;
            set => OnPropertyChanged(ref _imgList, value);
        }

        private bool _canResize;
        public bool CanResize
        {
            get => _canResize;
            set => OnPropertyChanged(ref _canResize, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => OnPropertyChanged(ref _isLoading, value);
        }

        private string _sourceDir;
        public string SourceDir
        {
            get => _sourceDir;
            set => OnPropertyChanged(ref _sourceDir, value);
        }

        private string _targetDir;
        public string TargetDir
        {
            get => _targetDir;
            set => OnPropertyChanged(ref _targetDir, value);
        }

        private int _imgHeight;
        public int ImgHeight
        {
            get => _imgHeight;
            set => OnPropertyChanged(ref _imgHeight, value);
        }

        private int _imgWidth;
        public int ImgWidth
        {
            get => _imgWidth;
            set
            {
                OnPropertyChanged(ref _imgWidth, value);
                if (SourceDir is not null || TargetDir is not null)
                    CanResize = true;
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => OnPropertyChanged(ref _errorMessage, value);
        }

        private bool _showErrorMessage;
        public bool ShowErrorMessage
        {
            get => _showErrorMessage;
            set => OnPropertyChanged(ref _showErrorMessage, value);
        }

        private bool _setRatio;
        public bool SetRatio
        {
            get => _setRatio;
            set => OnPropertyChanged(ref _setRatio, value);
        }

        private bool _openDirOnCompetion;
        public bool OpenDirOnCompletion
        {
            get => _openDirOnCompetion;
            set => OnPropertyChanged(ref _openDirOnCompetion, value);
        }

        private bool _saveAsPng;
        public bool SaveAsPng
        {
            get => _saveAsPng;
            set => OnPropertyChanged(ref _saveAsPng, value);
        }

        private bool _saveAsJpg;
        public bool SaveAsJpg
        {
            get => _saveAsJpg;
            set => OnPropertyChanged(ref _saveAsJpg, value);
        }

        public ResizeViewModel(IMessageService messageService)
        {
            _messageService = messageService;
            SourceDirCommand = new RelayCommand(SetSourceDir);
            TargetDirCommand = new RelayCommand(SetTargetDir);
            ClearInputCommand = new RelayCommand(ClearInputs);
            ResizeCommand = new RelayCommand(DoResize);
            CloseErrorMessageCommand = new RelayCommand(CloseErrorMessage);


        }

        private void CloseErrorMessage()
        {
            ErrorMessage = "";
            ShowErrorMessage = false;
        }

        private void DoResize()
        {
            if (SourceDir is not null && SourceDir != ""
                && TargetDir is not null && TargetDir != ""
                    && ImgWidth > 0 && ImgHeight > 0)
            {
                Task.Run(async () =>
                {
                    List<string> convertedList = new List<string>();
                    foreach (var item in ImgList)
                    {
                        convertedList.Add(item);
                    }

                    await Resize(convertedList, ImgWidth, ImgHeight, SetRatio, OpenDirOnCompletion);
                    if (OpenDirOnCompletion is true)
                        Process.Start("explorer.exe", TargetDir);
                });
            }
            else
            {
                ErrorMessage = "All Fields Are Required!";
                ShowErrorMessage = true;
            }
               
        }

        private void ClearInputs()
        {
            SourceDir = "";
            TargetDir = "";
            ImgWidth = 0;
            ImgHeight = 0;
            SetRatio = false;
            OpenDirOnCompletion = false;

            if(ImgList != null)
                ImgList.Clear();
        }

        private void SetTargetDir()
        {
            var OFD = new VistaFolderBrowserDialog();
            OFD.ShowNewFolderButton = true;

            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
                return;

            if ((bool?)OFD.ShowDialog() == true)
            {
                TargetDir = OFD.SelectedPath;
            }
        }

        private void SetSourceDir()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                await Task.WhenAll(LoadImages()).ContinueWith(t => IsLoading = false);
              
            });
        }

        private Task LoadImages()
        { 
            var OFD = new VistaFolderBrowserDialog();
            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
                return Task.CompletedTask;

            if ((bool?)OFD.ShowDialog() == true)
            {
                var selectedPath = OFD.SelectedPath;
                SourceDir = selectedPath;

                ImgList = new ObservableCollection<string>();
                var files = Directory.GetFiles(SourceDir);
                //int i = 0;
                   
                for (int i = 0; i <= files.Length - 1; i++)
                {
                    ImgList.Add(files[i]);
                }   
            }
            return Task.CompletedTask;
        }

        private async Task<bool> Resize(List<string> imgList, int newWidth, int newHeight, bool keepRatio, bool openTargetDir)
        {
            if (imgList == null)
                return await Task.FromResult(false);

            if (keepRatio)
            {
                IsLoading = true;
                foreach (var path in imgList)
                {
                    using (var image = Image.Load(path))
                    {
                        var img = image.CloneAs<Argb32>();
                        var index = path.LastIndexOf('\\');
                        var extIndex = path.LastIndexOf('.');
                        var ext = path.Substring(extIndex + 1);
                        var fName = path.Substring(index + 1).Replace(ext, string.Empty);

                        img.Mutate(x => x.Resize(new ResizeOptions()
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(newWidth, newHeight),
                        }));

                        if (SaveAsJpg)
                            img.SaveAsJpeg($"{TargetDir}\\{fName}.jpg");
                        if (SaveAsPng)
                            img.SaveAsJpeg($"{TargetDir}\\{fName}.png");
                    }
                }
                
            }
            else
            {
                IsLoading = true;
                foreach (var path in imgList)
                {
                    using (var image = Image.Load(path))
                    {
                        var img = image.CloneAs<Argb32>();
                        var index = path.LastIndexOf('\\');
                        var extIndex = path.LastIndexOf('.');
                        var ext = path.Substring(extIndex + 1);
                        var fName = path.Substring(index + 1).Replace(ext, string.Empty);

                        img.Mutate(x => x.Resize(new ResizeOptions()
                        {
                            Mode = ResizeMode.Manual,
                            Size = new Size(newWidth, newHeight),
                        }));

                        if (SaveAsJpg)
                            img.SaveAsJpeg($"{TargetDir}\\{fName}.jpg");
                        if (SaveAsPng)
                            img.SaveAsJpeg($"{TargetDir}\\{fName}.png");
                    }
                }

            }
            IsLoading = false;
            return await Task.FromResult(true);
        }
    }
}
