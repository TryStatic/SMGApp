using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SMGApp.WPF.Commands;
using SMGApp.WPF.ViewModels;

namespace SMGApp.WPF.States.Navigators
{
    public class Navigator : INavigator
    {
        public ViewModelBase CurrentViewModel { get; set; }
        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);
    }
}
