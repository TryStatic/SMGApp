using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;

namespace SMGApp.WPF.ViewModels
{
    public class ServiceViewModel : ViewModelBase
    {
        private readonly IDataService<ServiceItem> _serviceItemsDataService;

        private IEnumerable<ServiceItem> _serviceItems;
        private string _searchBox;

        public IEnumerable<ServiceItem> ServiceItems
        {
            get => _serviceItems;
            set
            {
                _serviceItems = value;
                this.OnPropertyChanged(nameof(ServiceItems));
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

        public ServiceViewModel(IDataService<ServiceItem> serviceItemsDataService)
        {
            this._serviceItemsDataService = serviceItemsDataService;
            Task.Run(async () => await LoadServiceItems());
        }


        public async Task LoadServiceItems() => ServiceItems = await _serviceItemsDataService.GetAll();

        private async void SearchBoxChanged(string value)
        {
            List<ServiceItem> serviceItems = (await _serviceItemsDataService.GetAll()).ToList();

            if (int.TryParse(value, out int id))
            {
                ServiceItems = serviceItems.Where(si => si.ID == id);
            }
            else
            { 
                ServiceItems = serviceItems.Where(c => c.CustomerDetails.ToLower().Contains(value.ToLower())).ToList();
            }
        }
    }
}