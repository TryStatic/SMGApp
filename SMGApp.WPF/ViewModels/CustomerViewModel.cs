using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;

namespace SMGApp.WPF.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        private readonly IDataService<Customer> _customerDataService;

        private IEnumerable<Customer> _customers;
        private string _searchBox;

        public IEnumerable<Customer> Customers
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

        public CustomerViewModel(IDataService<Customer> customerDataService)
        {
            this._customerDataService = customerDataService;
            Task.Run(async () => await LoadCustomers());
        }

        private async Task LoadCustomers() => Customers = await _customerDataService.GetAll();
        private async void SearchBoxChanged(string value) => Customers = (await _customerDataService.GetAll()).Where(c => c.LastName.ToLower().Contains(value.ToLower()) || c.FirstName.ToLower().Contains(value.ToLower())).ToList();
    }
}  