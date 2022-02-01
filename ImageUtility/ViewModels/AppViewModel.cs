using ImageUtility.Interfaces;
using ImageUtility.Utility.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageUtility.ViewModels
{
    internal class AppViewModel: BaseViewModel
    {
        private readonly INavigator? _navigator;
        public BaseViewModel? CurrentViewModel => _navigator.CurrentViewModel;
        public ICommand ExitAppCommand { get; set; }
        public ICommand NavigateSettingsCommand { get; }

        public AppViewModel(INavigator navigator)
        {
            _navigator = navigator;
            _navigator.CurrentViewModelChanged += OnCurrentViewModelChanged;
            ExitAppCommand = new RelayCommand(ExitApp);
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(_navigator, () => new SettingsViewModel());
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void ExitApp()
        {
            Environment.Exit(0);
        }
    }
}
