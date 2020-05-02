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
using SMGApp.WPF.Dialogs.CustomerDialogs;
using SMGApp.WPF.Dialogs.ServiceDialogs;

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
                ServiceItems = serviceItems.Where(c => c.CustomerDetails.ToUpper().Contains(value.ToUpper())).ToList();
            }
        }

        #region DeleteServiceEntry
        public ICommand DeleteServiceEntryCommand => new DialogCommand(DeleteServiceEntryDialog);
        private async void DeleteServiceEntryDialog(object idObject)
        {
            int id = (int)idObject;

            ServiceItem serviceEntry = await _serviceItemsDataService.Get(id);

            if (serviceEntry == null)
            {
                // Show error
                return;
            }

            DeleteServiceItemDialogView view = new DeleteServiceItemDialogView();
            DeleteServiceItemDialogViewModel viewmodel = new DeleteServiceItemDialogViewModel();

            viewmodel.ProductName = serviceEntry.DeviceDescription;
            viewmodel.ServiceEntryID = serviceEntry.ID;
            viewmodel.RelatedCustomer = serviceEntry.CustomerDetails;
            
            view.DataContext = viewmodel;

            //show the dialog
            object result = await DialogHost.Show(view, "RootDialog", OneDeleteUserDialogOpen, OneDeleteUserDialogClose);
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void OneDeleteUserDialogOpen(object sender, DialogOpenedEventArgs eventargs)
        {
            
        }

        private async void OneDeleteUserDialogClose(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            // Get user details
            if (eventArgs.Session.Content is DeleteServiceItemDialogView deleteServiceItemDialogView && deleteServiceItemDialogView.DataContext is DeleteServiceItemDialogViewModel model)
            {
                eventArgs.Session.UpdateContent(new ProgressDialog());

                await _serviceItemsDataService.Delete(model.ServiceEntryID);
                
                await LoadServiceItems();
            }
            else
            {
                // show error
            }

            await LoadServiceItems().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}