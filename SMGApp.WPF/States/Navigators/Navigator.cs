using System.Windows.Input;
using SMGApp.WPF.Commands;
using SMGApp.WPF.Models;
using SMGApp.WPF.ViewModels;
using SMGApp.WPF.ViewModels.Factories;

namespace SMGApp.WPF.States.Navigators
{
    public class Navigator : ObservableObject, INavigator
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand UpdateCurrentViewModelCommand { get; set; }

        public Navigator(IRootSMGAppViewModelAbstractFactory viewModelFactory)
        {
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(this, viewModelFactory);
        }
    }
}
