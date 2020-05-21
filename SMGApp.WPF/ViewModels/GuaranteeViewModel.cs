using SMGApp.EntityFramework.Services;

namespace SMGApp.WPF.ViewModels
{
    public class GuaranteeViewModel : ViewModelBase
    {
        private readonly CustomersDataService _customerService;
        private readonly GuaranteeDataService _guaranteeDataService;

        public GuaranteeViewModel(CustomersDataService customerService, GuaranteeDataService guaranteeDataService)
        {
            _customerService = customerService;
            _guaranteeDataService = guaranteeDataService;
        }
    }
}