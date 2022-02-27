using ImageUtility.Interfaces;
using ImageUtility.Utility.Commands;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageUtility.ViewModels
{
    internal class ResizeViewModel: BaseViewModel
    {
        private IMessageService _messageService;
        public ICommand SourceDirCommand { get; set; }
        public ICommand TargetDirCommand { get; set;}
        public ICommand ResizeCommand { get; set;}
        public ICommand ClearInputCommand { get; set; }



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

        public ResizeViewModel(IMessageService messageService)
        {
            _messageService = messageService;
            SourceDirCommand = new RelayCommand(SetSourceDir);
            TargetDirCommand = new RelayCommand(SetTargetDir);
            
        }


        private void SetTargetDir()
        {
            var OFD = new VistaFolderBrowserDialog();
            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
                return;

            if ((bool)OFD.ShowDialog() == true)
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

        private async Task LoadImages()
        { 
            var OFD = new VistaFolderBrowserDialog();
            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
                return;

            if ((bool)OFD.ShowDialog() == true)
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
           
        }
    }
}
