using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using SMGApp.Domain.Models;
using SMGApp.EntityFramework.Services;
using SMGApp.WPF.Commands;
using SMGApp.WPF.Dialogs;
using SMGApp.WPF.Dialogs.CustomerDialogs;
using SMGApp.WPF.ViewModels.Util;

namespace SMGApp.WPF.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        private readonly CustomersDataService _customerDataService;
        private readonly ServiceItemsDataService _serviceItemsDataService;

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

        public CustomerViewModel(CustomersDataService customerDataService, ServiceItemsDataService serviceItemsDataService)
        {
            _customerDataService = customerDataService;
            _serviceItemsDataService = serviceItemsDataService;
            Task.Run(async () => await LoadCustomers());
        }

        public ICommand DeleteUserCommand => new DialogCommand(ExecuteDeleteUserDialog);
        private async void ExecuteDeleteUserDialog(object idObject)
        {
            int id = (int)idObject;

            Customer customer = await _customerDataService.Get(id);
            List<ServiceItem> entries = (await _serviceItemsDataService.GetByCustomer(id)).ToList();

            if (customer == null)
            {
                // Show error
                return;
            }

            DeleteDialogView view = new DeleteDialogView();
            DeleteViewModel viewmodel = new DeleteViewModel();

            viewmodel.CustomerID = customer.ID;
            viewmodel.ServiceEntries = entries;

            viewmodel.DeleteCustomerName = $"{customer.FirstName} {customer.LastName}";
            viewmodel.DeleteCustomerServiceEntries = entries.Count == 0 ? "ΚΑΜΙΑ" : entries.Count.ToString();

            viewmodel.DeleteCustomerWarning = "";
            if (entries.Count > 0)
            {
                viewmodel.DeleteCustomerWarning = "ΠΡΟΣΟΧΗ: ΔΙΑΓΡΑΦΟΝΤΑΣ ΤΟΝ ΠΕΛΑΤΗ ΘΑ ΔΙΑΓΡΑΦΟΥΝ ΚΑΙ ΟΙ ΑΝΤΙΣΤΟΙΧΕΣ ΕΙΣΑΓΩΓΕΣ ΣΤΟ SERVICE";
            }
            view.DataContext = viewmodel;

            //show the dialog
            object result = await DialogHost.Show(view, "RootDialog", OneDeleteUserDialogOpen, OneDeleteUserDialogClose);
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void OneDeleteUserDialogOpen(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private async void OneDeleteUserDialogClose(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            // Get user details
            if (eventArgs.Session.Content is DeleteDialogView addNewUserDialogView && addNewUserDialogView.DataContext is DeleteViewModel model)
            {
                eventArgs.Session.UpdateContent(new ProgressDialog());

                foreach (ServiceItem entry in model.ServiceEntries) await _serviceItemsDataService.Delete(entry.ID);
                
                await _customerDataService.Delete(model.CustomerID);

                Console.WriteLine($"Deleted {model.ServiceEntries} and customer with ID {model.CustomerID}");

                await LoadCustomers();
            }
            else
            {
                // show error
            }
            await LoadCustomers().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #region EditUserDialog
        public ICommand EditUserCommand => new DialogCommand(ExecuteUpdateUserDialog);

        private async void ExecuteUpdateUserDialog(object idObject)
        {
            int id = (int)idObject;

            Customer customer = await _customerDataService.Get(id);

            if (customer == null)
            {
                // show error
                return;
            }



            //let's set up a little MVVM, cos that's what the cool kids are doing:
            UserDialogView view = new UserDialogView();

            UserViewModel viewmodel = new UserViewModel();

            viewmodel.OperationName = $"ΕΝΗΜΕΡΩΣΗ ΣΤΟΙΧΕΙΩΝ ΠΕΛΑΤΗ (ID: {id})";
            viewmodel.FirstName = customer.FirstName;
            viewmodel.LastName = customer.LastName;
            viewmodel.Address = customer.Address;
            viewmodel.Note = customer.Notes;
            viewmodel.PhoneNumber = customer.PhoneNumber;

            viewmodel.UpdateID = customer.ID;
            viewmodel.UpdateDateAdded = customer.DateAdded;

            view.DataContext = viewmodel;


            //show the dialog
            object result = await DialogHost.Show(view, "RootDialog", OnUpdateUserDialogOpen, OnUpdateUserDialogClose);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void OnUpdateUserDialogOpen(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private async void OnUpdateUserDialogClose(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            // Get user details
            if (eventArgs.Session.Content is UserDialogView addNewUserDialogView && addNewUserDialogView.DataContext is UserViewModel model)
            {
                eventArgs.Session.UpdateContent(new ProgressDialog());

                if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
                {
                    // show error
                }
                else
                {
                    Customer newCustomerDetails = new Customer()
                    {
                        FirstName = model.FirstName.ToUpperΝοintonation(),
                        LastName = model.LastName.ToUpperΝοintonation(),
                        Address = model.Address.ToUpperΝοintonation(),
                        DateAdded = model.UpdateDateAdded,
                        Notes = model.Note.ToUpperΝοintonation(),
                        PhoneNumber = model.PhoneNumber.ToUpperΝοintonation()
                    };
                    await _customerDataService.Update(model.UpdateID, newCustomerDetails);
                }
            }
            else
            {
                // show error
            }
            await LoadCustomers().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region CreateUserDialog
        public ICommand CreateNewUserCommand => new DialogCommand(ExecuteAddNewUserDialog);
        private async void ExecuteAddNewUserDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            UserDialogView view = new UserDialogView()
            {
                DataContext = new UserViewModel()
                {
                    OperationName = "ΕΙΣΑΓΩΓΗ ΝΕΟΥ ΠΕΛΑΤΗ"
                }
            };

            //show the dialog
            object result = await DialogHost.Show(view, "RootDialog", OnCreateNewUserDialogOpen, OnCreateNewUserDialogClose);
            
            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void OnCreateNewUserDialogOpen(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private async void OnCreateNewUserDialogClose(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            // Get user details
            if (eventArgs.Session.Content is UserDialogView addNewUserDialogView && addNewUserDialogView.DataContext is UserViewModel model)
            {
                eventArgs.Session.UpdateContent(new ProgressDialog());

                if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
                {
                    // show error
                }
                else
                {
                    Customer newCustomer = new Customer()
                    {
                        FirstName = model.FirstName.ToUpperΝοintonation(),
                        LastName = model.LastName.ToUpperΝοintonation(),
                        Address = model.Address.ToUpperΝοintonation(),
                        DateAdded = DateTime.Now,
                        Notes = model.Note.ToUpperΝοintonation(),
                        PhoneNumber = model.PhoneNumber.ToUpperΝοintonation()
                    };
                    await _customerDataService.Create(newCustomer);
                }
            }
            else
            {
                // show error
            }
            await LoadCustomers().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion


        public async Task LoadCustomers() => Customers = await _customerDataService.GetAll();
        private async void SearchBoxChanged(string value) => Customers = (await _customerDataService.GetAll()).Where(c => c.LastName.ToLower().Contains(value.ToLower()) || c.FirstName.ToLower().Contains(value.ToLower())).ToList();
    }
}  