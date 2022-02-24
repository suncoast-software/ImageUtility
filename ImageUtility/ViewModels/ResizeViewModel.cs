using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageUtility.ViewModels
{
    internal class ResizeViewModel: BaseViewModel
    {
        public ICommand SourceDirCommand { get; set; }
        public ICommand TargetDirCommand { get; set;}

        public ResizeViewModel()
        {

        }
    }
}
