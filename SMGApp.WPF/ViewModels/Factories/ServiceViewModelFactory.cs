namespace SMGApp.WPF.ViewModels.Factories
{
    public class ServiceViewModelFactory : ISMGAppViewModelFactory<ServiceViewModel>
    {
        public ServiceViewModel CreateViewModel()
        {
            return new ServiceViewModel();
        }
    }
}