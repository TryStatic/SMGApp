using SMGApp.WPF.States.Navigators;

namespace SMGApp.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public INavigator Navigator { get; set; }

        public MainViewModel(INavigator navigator)
        {
            Navigator = navigator;

            // Startup View
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Customer);
        }
    }
}
