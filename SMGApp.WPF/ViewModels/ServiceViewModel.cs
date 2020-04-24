using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;
using SMGApp.WPF.Commands;
using SMGApp.WPF.Dialogs;
using SMGApp.WPF.ViewModels.Util;

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
            // Implement
        }
    }
}