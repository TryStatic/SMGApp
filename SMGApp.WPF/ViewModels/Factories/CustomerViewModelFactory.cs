using System;
using System.Collections.Generic;
using System.Text;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;

namespace SMGApp.WPF.ViewModels.Factories
{
    public class CustomerViewModelFactory : ISMGAppViewModelFactory<CustomerViewModel>
    {
        private readonly IDataService<Customer> _customerService;

        public CustomerViewModelFactory(IDataService<Customer> customerService)
        {
            _customerService = customerService;
        }

        public CustomerViewModel CreateViewModel()
        {
            return new CustomerViewModel(_customerService);
        }
    }
}
