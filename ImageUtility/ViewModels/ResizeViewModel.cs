using ImageUtility.Interfaces;
using ImageUtility.Utility.Commands;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        private string _sourceDir;
        public string SourceDir
        {
            get => _sourceDir;
            set => OnPropertyChanged(ref _sourceDir, value);
        }

        public ResizeViewModel(IMessageService messageService)
        {
            _messageService = messageService;
            SourceDirCommand = new RelayCommand(SetSourceDir);
            TargetDirCommand = new RelayCommand(SetTargetDir);
            
        }

        private void SetTargetDir()
        {
            throw new NotImplementedException();
        }

        private void SetSourceDir()
        {
            ImgList = new ObservableCollection<string>();
            var OFD = new VistaFolderBrowserDialog();
            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
                return;

            if ((bool)OFD.ShowDialog() == true)
            {
                var selectedPath = OFD.SelectedPath;
                SourceDir = selectedPath;

                var files = Directory.GetFiles(selectedPath);
                foreach (var file in files)
                {
                    ImgList.Add(file);
                } 
            }
        }
    }
}
