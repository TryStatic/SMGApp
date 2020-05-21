using SMGApp.EntityFramework.Services;

namespace SMGApp.WPF.ViewModels.Factories
{
    public class GuaranteeViewModelFactory : ISMGAppViewModelFactory<GuaranteeViewModel>
    {
        private readonly CustomersDataService _customerService;
        private readonly GuaranteeDataService _guaranteeDataService;

        public GuaranteeViewModelFactory(CustomersDataService customerService, GuaranteeDataService guaranteeDataService)
        {
            _customerService = customerService;
            _guaranteeDataService = guaranteeDataService;
        }

        public GuaranteeViewModel CreateViewModel()
        {
            return new GuaranteeViewModel(_customerService, _guaranteeDataService);
        }
    }
}