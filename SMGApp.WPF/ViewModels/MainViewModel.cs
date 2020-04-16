using SMGApp.WPF.States.Navigators;

namespace SMGApp.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public INavigator Navigator { get; set; } = new Navigator();

        public MainViewModel()
        {
            // Startup View
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Customer);
        }
    }
}
