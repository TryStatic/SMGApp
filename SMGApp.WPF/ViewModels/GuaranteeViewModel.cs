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
using SMGApp.WPF.Dialogs.ServiceDialogs;

namespace SMGApp.WPF.ViewModels
{
    public class GuaranteeViewModel : ViewModelBase
    {
        private readonly CustomersDataService _customerService;
        private readonly GuaranteeDataService _guaranteeDataService;

        public GuaranteeViewModel(CustomersDataService customerService, GuaranteeDataService guaranteeDataService)
        {
            _customerService = customerService;
            _guaranteeDataService = guaranteeDataService;

            Task.Run(async () => await LoadGuaranties());
        }



        #region DeleteGuaranteeEntry
        public ICommand DeleteGuaranteeEntryCommand => new DialogCommand(DeleteGuaranteeEntryDialog);
        private async void DeleteGuaranteeEntryDialog(object idObject)
        {
            int id = (int)idObject;

            Guarantee guarantee = await _guaranteeDataService.Get(id);

            if (guarantee == null)
            {
                // Show error
                return;
            }

            DeleteItemDialogView view = new DeleteItemDialogView();
            DeleteItemDialogViewModel viewmodel = new DeleteItemDialogViewModel();

            viewmodel.ProductName = guarantee.ProductDesc;
            viewmodel.ServiceEntryID = guarantee.ID;
            viewmodel.RelatedCustomer = guarantee.CustomerDetails;
            viewmodel.RelatedTab = "GUARANTEE";

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
            if (eventArgs.Session.Content is DeleteItemDialogView deleteServiceItemDialogView && deleteServiceItemDialogView.DataContext is DeleteItemDialogViewModel model)
            {
                eventArgs.Session.UpdateContent(new ProgressDialog());

                await _guaranteeDataService.Delete(model.ServiceEntryID);

                await LoadGuaranties();
            }
            else
            {
                // show error
            }

            await LoadGuaranties().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region SearchBox
        private string _searchBox;
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
            if (value == null)
            {
                await LoadGuaranties();
                return;
            }

            if (int.TryParse(value, out int id))
            {
                GuaranteeEntries = (await _guaranteeDataService.GetAll()).OrderByDescending(r => r.ID).Where(i => i.ID == id).ToList();
            }
            else
            {
                GuaranteeEntries = (await _guaranteeDataService.GetAll()).Where(c => c.CustomerDetails.ToUpper().Contains(value.ToUpper())).OrderByDescending(r => r.ID).ToList();
            }
        }
        #endregion

        #region GuaranteeEntries
        private IEnumerable<Guarantee> _guaranteeEntries;
        public IEnumerable<Guarantee> GuaranteeEntries
        {
            get => _guaranteeEntries;
            set
            {
                _guaranteeEntries = value;
                OnPropertyChanged(nameof(GuaranteeEntries));
            }
        }

        private async Task LoadGuaranties()
        {
            GuaranteeEntries = (await _guaranteeDataService.GetAll()).OrderByDescending(r => r.ID).ToList();
        }

        #endregion


    }
}