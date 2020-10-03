using SMGApp.EntityFramework.Services;

namespace SMGApp.WPF.ViewModels.Factories
{
    public class ServiceViewModelFactory : ISMGAppViewModelFactory<ServiceViewModel>
    {
        private readonly ServiceItemsDataService _serviceItemService;
        private readonly CustomersDataService _customerServiceDataService;

        public ServiceViewModelFactory(ServiceItemsDataService serviceItemService, CustomersDataService customerServiceDataService)
        {
            _serviceItemService = serviceItemService;
            _customerServiceDataService = customerServiceDataService;
        }


        public ServiceViewModel CreateViewModel()
        {
            return new ServiceViewModel(_serviceItemService, _customerServiceDataService);
        }
    }
}