using System;
using System.Windows.Input;
using SMGApp.WPF.States.Navigators;
using SMGApp.WPF.ViewModels.Factories;

namespace SMGApp.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {

        private readonly INavigator _navigator;
        private readonly IRootSMGAppViewModelAbstractFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(INavigator navigator, IRootSMGAppViewModelAbstractFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!(parameter is ViewType viewType)) return;

            _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
