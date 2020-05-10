using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using SMGApp.Domain.Models;
using SMGApp.EntityFramework.Services;
using SMGApp.WPF.Commands;
using SMGApp.WPF.Dialogs;
using SMGApp.WPF.Dialogs.ServiceDialogs;

namespace SMGApp.WPF.ViewModels
{
    public class ServiceViewModel : ViewModelBase
    {
        private readonly ServiceItemsDataService _serviceItemsDataService;
        private readonly CustomersDataService _customerServiceDataService;

        private IEnumerable<ServiceItem> _serviceItems;
        private string _searchBox;
        private bool _arrivedCheckbox = true;
        private bool _fixedCheckbox = true;
        private bool _deliveredCheckbox = true;
        private bool _issueCheckbox = true;

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

        public bool ArrivedCheckbox
        {
            get => _arrivedCheckbox;
            set
            {
                _arrivedCheckbox = value;
                this.OnPropertyChanged(nameof(ArrivedCheckbox));
                SearchBoxChanged(SearchBox);
            }
        }

        public bool FixedCheckbox
        {
            get => _fixedCheckbox;
            set
            {
                _fixedCheckbox = value;
                this.OnPropertyChanged(nameof(FixedCheckbox));
                SearchBoxChanged(SearchBox);
            }
        }

        public bool DeliveredCheckbox
        {
            get => _deliveredCheckbox;
            set
            {
                _deliveredCheckbox = value;
                this.OnPropertyChanged(nameof(DeliveredCheckbox));
                SearchBoxChanged(SearchBox);
            }
        }

        public bool IssueCheckbox
        {
            get => _issueCheckbox;
            set
            {
                _issueCheckbox = value;
                this.OnPropertyChanged(nameof(IssueCheckbox));
                SearchBoxChanged(SearchBox);
            }
        }

        public ICommand ResetFiltersCommand => new GenericCommand(o =>
        {
            ArrivedCheckbox = true;
            FixedCheckbox = true;
            DeliveredCheckbox = true;
            IssueCheckbox = true;
        });

        private Visibility _hideDateColumns = Visibility.Hidden;
        public Visibility HideDateColumns
        {
            get => _hideDateColumns;
            set
            {
                _hideDateColumns = value;
                this.OnPropertyChanged(nameof(HideDateColumns));
            }
        }   

        private bool _hideDateColumnsBool = true;
        public bool HideDateColumnsBool
        {
            get => _hideDateColumnsBool;
            set
            {
                HideDateColumns = value ? Visibility.Hidden : Visibility.Visible;
                _hideDateColumnsBool = value;
                this.OnPropertyChanged(nameof(HideDateColumnsBool));
            }
        }


        public ServiceViewModel(ServiceItemsDataService serviceItemsDataService, CustomersDataService customerServiceDataService)
        {
            _serviceItemsDataService = serviceItemsDataService;
            _customerServiceDataService = customerServiceDataService;
            Task.Run(async () => await LoadServiceItems());
        }

        public async Task LoadServiceItems()
        {
            ServiceItems = (await GetFilteredServiceItems()).OrderByDescending(x => x.ID);
        }

        private async Task<IEnumerable<ServiceItem>> GetFilteredServiceItems()
        {
            List<ServiceItem> allItems = (await _serviceItemsDataService.GetAll()).ToList();
            List<ServiceItem> itemsShown = new List<ServiceItem>();

            if (ArrivedCheckbox && FixedCheckbox && DeliveredCheckbox && IssueCheckbox) return allItems;

            if (ArrivedCheckbox)
            {
                IEnumerable<ServiceItem> items = allItems.Where(it => it.State == ServiceState.Arrived);
                itemsShown.AddRange(items);
            }
            if (FixedCheckbox)
            {
                IEnumerable<ServiceItem> items = allItems.Where(it => it.State == ServiceState.Fixed);
                itemsShown.AddRange(items);
            }
            if (DeliveredCheckbox)
            {
                IEnumerable<ServiceItem> items = allItems.Where(it => it.State == ServiceState.Delivered);
                itemsShown.AddRange(items);
            }
            if (IssueCheckbox)
            {
                IEnumerable<ServiceItem> items = allItems.Where(it => it.State == ServiceState.Issue);
                itemsShown.AddRange(items);
            }

            return itemsShown;
        }

        private async void SearchBoxChanged(string value)
        {
            List<ServiceItem> serviceItems = (await GetFilteredServiceItems()).ToList();

            if (value == null)
            {
                ServiceItems = serviceItems.OrderByDescending(x => x.ID);
                return;
            }

            if (int.TryParse(value, out int id))
            {
                ServiceItems = serviceItems.Where(si => si.ID == id);
            }
            else
            { 
                ServiceItems = serviceItems.Where(c => c.CustomerDetails.ToUpper().Contains(value.ToUpper())).ToList();
            }
        }

        #region CreateNewServiceItem
        public ICommand CreateNewServiceItemCommand => new DialogCommand(CreateNewServiceEntryDialog);
        private async void CreateNewServiceEntryDialog(object o)
        {

            ServiceDialogView view = new ServiceDialogView();
            ServiceDialogViewModel model = new ServiceDialogViewModel((await _customerServiceDataService.GetAll()).ToList().Select(item => item.CustomerDetails).ToList());

            model.OperationName = "ΕΙΣΑΓΩΓΗ ΝΕΟΥ SERVICE";

            view.DataContext = model;

            //show the dialog
            object result = await DialogHost.Show(view, "RootDialog", OnCreateNewServiceEntryDialog, OnCreateNewServiceEntryDialog);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void OnCreateNewServiceEntryDialog(object sender, DialogOpenedEventArgs eventargs)
        {

        }
        private async void OnCreateNewServiceEntryDialog(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            // Get user details
            if (eventArgs.Session.Content is ServiceDialogView deleteServiceItemDialogView && deleteServiceItemDialogView.DataContext is ServiceDialogViewModel model)
            {
                eventArgs.Session.UpdateContent(new ProgressDialog());

                ServiceItem newDetails = new ServiceItem();

                Customer customer = (await _customerServiceDataService.GetAll()).FirstOrDefault(c => c.CustomerDetails == model.CustomerName);
                if (customer == null)
                {
                    MessageBox.Show($"Ο ΠΕΛΑΤΗΣ {model.CustomerName} ΔΕΝ ΒΡΕΘΗΚΕ");
                    await LoadServiceItems().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
                    return;
                }

                newDetails.Customer = customer;
                newDetails.DeviceDescription = model.Device;
                newDetails.DamageDescription = model.DamageDescription;
                newDetails.Notes = model.Notes;
                newDetails.DevicePassword = model.DevicePassword;
                newDetails.SimPassword = model.SimCode;
                newDetails.DeviceAccountUsername = model.AccountUsername;
                newDetails.DeviceAccountPassword = model.AccountPassword;
                newDetails.ChargerIncluded = model.ChargerIncluded;
                newDetails.BagIncluded = model.BagIncluded;
                newDetails.CaseIncluded = model.CaseIncluded;
                newDetails.State = model.ServiceState;
                newDetails.Price = model.Cost;
                newDetails.DateUpdated = DateTime.Now;
                newDetails.DateAdded = DateTime.Now;

                await _serviceItemsDataService.Create(newDetails);
                await LoadServiceItems();
            }
            else
            {
                // show error
            }

            await LoadServiceItems().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion


        #region EditServiceItem
        public ICommand EditServiceItem => new DialogCommand(EditServiceEntryDialog);
        private async void EditServiceEntryDialog(object idObject)
        {
            int id = (int) idObject;

            ServiceItem serviceItem = await (_serviceItemsDataService.Get(id));
            if (serviceItem == null)
            {
                // Show Error
                return;
            }

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            ServiceDialogView view = new ServiceDialogView();
            ServiceDialogViewModel model = new ServiceDialogViewModel((await _customerServiceDataService.GetAll()).ToList().Select(item => item.CustomerDetails).ToList());

            model.UpdateID = id;
            
            model.CustomerBeforeEdit = serviceItem.Customer;
            model.CustomerName = serviceItem.CustomerDetails;

            model.Device = serviceItem.DeviceDescription;
            model.DamageDescription = serviceItem.DamageDescription;
            model.Notes = serviceItem.Notes;
            model.DevicePassword = serviceItem.DevicePassword;
            model.SimCode = serviceItem.SimPassword;
            model.AccountUsername = serviceItem.DeviceAccountUsername;
            model.AccountPassword = serviceItem.DeviceAccountPassword;
            model.ChargerIncluded = serviceItem.ChargerIncluded;
            model.BagIncluded = serviceItem.BagIncluded;
            model.CaseIncluded = serviceItem.CaseIncluded;
            model.ServiceState = serviceItem.State;
            model.CustomerBeforeEdit = serviceItem.Customer;
            model.Cost = serviceItem.Price;


            model.OperationName = $"ΕΠΕΞΕΡΓΑΣΙΑ ΕΙΣΑΓΩΓΗΣ SERVICE (ID: {id})";

            view.DataContext = model;

            //show the dialog
            object result = await DialogHost.Show(view, "RootDialog", OnEditServiceEntryDialog, OnEditServiceEntryDialog);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void OnEditServiceEntryDialog(object sender, DialogOpenedEventArgs eventargs)
        {

        }
        private async void OnEditServiceEntryDialog(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            // Get user details
            if (eventArgs.Session.Content is ServiceDialogView deleteServiceItemDialogView && deleteServiceItemDialogView.DataContext is ServiceDialogViewModel model)
            {

                eventArgs.Session.UpdateContent(new ProgressDialog());

                ServiceItem updatedDetails = new ServiceItem();

                if (model.CustomerBeforeEdit.CustomerDetails == model.CustomerName)
                {
                    updatedDetails.Customer = model.CustomerBeforeEdit;
                }
                else
                {
                    Customer customer = (await _customerServiceDataService.GetAll()).FirstOrDefault(c => c.CustomerDetails == model.CustomerName);
                    if (customer == null)
                    {
                        MessageBox.Show($"Ο ΠΕΛΑΤΗΣ {model.CustomerName} ΔΕΝ ΒΡΕΘΗΚΕ");
                        await LoadServiceItems().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
                        return;
                    }
                    updatedDetails.Customer = customer;
                }
                updatedDetails.DeviceDescription = model.Device;
                updatedDetails.DamageDescription = model.DamageDescription;
                updatedDetails.Notes = model.Notes;
                updatedDetails.DevicePassword = model.DevicePassword;
                updatedDetails.SimPassword = model.SimCode;
                updatedDetails.DeviceAccountUsername = model.AccountUsername;
                updatedDetails.DeviceAccountPassword = model.AccountPassword;
                updatedDetails.ChargerIncluded = model.ChargerIncluded;
                updatedDetails.BagIncluded = model.BagIncluded;
                updatedDetails.CaseIncluded = model.CaseIncluded;
                updatedDetails.State = model.ServiceState;
                updatedDetails.Price = model.Cost;
                updatedDetails.DateUpdated = DateTime.Now;

                await _serviceItemsDataService.Update(model.UpdateID, updatedDetails);

                await LoadServiceItems();
            }
            else
            {
                // show error
            }

            await LoadServiceItems().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

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

        public ICommand PrintReceiptCommand => new GenericCommand(o =>
        {
            // PrintReceiptCommand LOGIC
        });
    }
}