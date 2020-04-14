using System;
using System.Collections.Generic;
using System.Linq;
using SMGApp.Domain.Models;
using SMGApp.EntityFramework;
using SMGApp.EntityFramework.Services;

namespace SMGApp.WPF.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {

        public List<Customer> Customers { get; set; }

        public CustomerViewModel()
        {
            LoadCustomers();
        }

        private async void LoadCustomers()
        {
            GenericDataServices<Customer> service = new GenericDataServices<Customer>(new SMGAppDbContextFactory());
            IEnumerable<Customer> x = await service.GetAll();
            Customers = x.ToList();

        }
    }
}  