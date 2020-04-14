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
        private List<Customer> _customers;
        private string _searchBox;

        public List<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                this.OnPropertyChanged(nameof(Customers));
            }
        }

        public string SearchBox
        {
            get => _searchBox;
            set
            {
                _searchBox = value;
                this.OnPropertyChanged(nameof(SearchBox));
                SearchBoxChanged(value);
            }
        }

        private async void SearchBoxChanged(string value)
        {
            if(Customers == null) return;
            GenericDataServices<Customer> service = new GenericDataServices<Customer>(new SMGAppDbContextFactory());
            IEnumerable<Customer> x = await service.GetAll();
            List<Customer> cust = x.ToList();
            Customers = cust.Where(c => c.LastName.ToLower().Contains(value.ToLower()) || c.FirstName.ToLower().Contains(value.ToLower())).ToList();
        }

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