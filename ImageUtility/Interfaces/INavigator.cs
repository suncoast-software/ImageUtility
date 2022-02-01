using ImageUtility.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUtility.Interfaces
{
    internal interface INavigator
    {
        public event Action? CurrentViewModelChanged;

        public BaseViewModel? CurrentViewModel { get; set; }
    }
}
