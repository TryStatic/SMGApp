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

        #region CREATENEWUSERDIALOG
        public ICommand CreateNewUserCommand => new DialogCommand(ExecuteRunExtendedDialog);
        private async void ExecuteRunExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new SampleDialog
            {
                DataContext = new SampleDialogViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new SampleProgressDialog());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion


        private async Task LoadCustomers() => Customers = await _customerDataService.GetAll();
        private async void SearchBoxChanged(string value) => Customers = (await _customerDataService.GetAll()).Where(c => c.LastName.ToLower().Contains(value.ToLower()) || c.FirstName.ToLower().Contains(value.ToLower())).ToList();
    }
}  