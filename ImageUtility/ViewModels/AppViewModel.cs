﻿using ImageUtility.Interfaces;
using ImageUtility.Utility.Commands;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageUtility.ViewModels
{
    internal class AppViewModel: BaseViewModel
    {
        private readonly INavigator? _navigator;
        private readonly IMessageService _messageService;
        public BaseViewModel? CurrentViewModel => _navigator.CurrentViewModel;
        public ICommand ExitAppCommand { get; set; }
        public ICommand NavigateSettingsCommand { get; }
        public ICommand NavigateResizeCommand { get; }

        public AppViewModel(INavigator navigator, IMessageService messageService)
        {
            _navigator = navigator;
            _messageService = messageService;
            _navigator.CurrentViewModelChanged += OnCurrentViewModelChanged;
            ExitAppCommand = new RelayCommand(ExitApp);
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(_navigator, () => new SettingsViewModel());
            NavigateResizeCommand = new NavigateCommand<ResizeViewModel>(_navigator, () => new ResizeViewModel(_messageService));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void ExitApp()
        {
            App.Current.Shutdown();
        }
    }
}
