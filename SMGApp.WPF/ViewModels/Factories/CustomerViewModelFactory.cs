using System;
using System.Collections.Generic;
using System.Text;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;
using SMGApp.EntityFramework.Services;

namespace SMGApp.WPF.ViewModels.Factories
{
    public class CustomerViewModelFactory : ISMGAppViewModelFactory<CustomerViewModel>
    {
        private readonly CustomersDataService _customerService;
        private readonly ServiceItemsDataService _serviceItemsDataService;

        public CustomerViewModelFactory(CustomersDataService customerService, ServiceItemsDataService serviceItemsDataService)
        {
            _customerService = customerService;
            _serviceItemsDataService = serviceItemsDataService;
        }


        public CustomerViewModel CreateViewModel()
        {
            return new CustomerViewModel(_customerService, _serviceItemsDataService);
        }
    }
}
