﻿using System;
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
using SMGApp.WPF.Dialogs.GuaranteeDialogs;

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

            ShowExpired = App.CheckBoxesStates.GuaranteeHide;


            Task.Run(async () => await LoadGuaranties());
        }


        private bool _showExpired;

        public bool ShowExpired
        {
            get => _showExpired;
            set
            {
                _showExpired = value;
                this.OnPropertyChanged(nameof(ShowExpired));
                SearchBoxChanged(SearchBox);
                App.CheckBoxesStates.GuaranteeHide = value;
            }
        }


        #region CreateNewGuaranteeItem
        public ICommand CreateNewGuaranteeItemCommand => new DialogCommand(CreateNewServiceEntryDialog);
        private async void CreateNewServiceEntryDialog(object o)
        {

            GuaranteeDialogView view = new GuaranteeDialogView();
            List<string> list = (await _customerService.GetAll()).ToList().Select(item => item.CustomerDetails).ToList();
            list.Insert(0, "KENO");
            GuaranteeDialogViewModel model = new GuaranteeDialogViewModel(list);

            model.OperationName = "ΕΙΣΑΓΩΓΗ ΝΕΟΥ GUARANTEE";
            model.StartDate = DateTime.Now;
            model.EndDate = DateTime.Now;

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
            if (eventArgs.Session.Content is GuaranteeDialogView deleteServiceItemDialogView && deleteServiceItemDialogView.DataContext is GuaranteeDialogViewModel model)
            {
                eventArgs.Session.UpdateContent(new ProgressDialog());

                Guarantee newDetails = new Guarantee();

                Customer customer = (await _customerService.GetAll()).FirstOrDefault(c => c.CustomerDetails == model.CustomerName);

                if (model.EndDate <= model.StartDate)
                {
                    MessageBox.Show("Invalid Dates");
                    return;
                }

                newDetails.Customer = customer;
                newDetails.ProductDesc = model.Product;
                newDetails.ProductIMEI = model.IMEI;
                newDetails.ProductNotes = model.Notes;
                newDetails.StartDate = model.StartDate;
                newDetails.EndDate = model.EndDate;
                newDetails.GuaranteeType = model.GuaranteeType;

                newDetails.VirtualID = await _guaranteeDataService.GetVirtualID(newDetails.GuaranteeType);

                await _guaranteeDataService.Create(newDetails);
                await LoadGuaranties();
            }
            else
            {
                // show error
            }

            await LoadGuaranties().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region EditGuaranteeEntry
        public ICommand EditGuaranteeItem => new DialogCommand(EditServiceEntryDialog);
        private async void EditServiceEntryDialog(object idObject)
        {
            int id = (int)idObject;

            Guarantee guaranteeEntry = await (_guaranteeDataService.Get(id));


            //let's set up a little MVVM, cos that's what the cool kids are doing:
            GuaranteeDialogView view = new GuaranteeDialogView();
            List<string> list = (await _customerService.GetAll()).ToList().Select(item => item.CustomerDetails).ToList();
            list.Insert(0, "KENO");
            GuaranteeDialogViewModel model = new GuaranteeDialogViewModel(list);

            model.UpdateID = id;

            model.CustomerBeforeEdit = guaranteeEntry.Customer;
            model.CustomerName = guaranteeEntry.CustomerDetails;

            model.Product = guaranteeEntry.ProductDesc;
            model.IMEI = guaranteeEntry.ProductIMEI;
            model.Notes = guaranteeEntry.ProductNotes;
            model.StartDate = guaranteeEntry.StartDate;
            model.EndDate = guaranteeEntry.EndDate;
            model.GuaranteeType = guaranteeEntry.GuaranteeType;

            model.OperationName = $"ΕΠΕΞΕΡΓΑΣΙΑ ΕΙΣΑΓΩΓΗΣ GUARANTEE (ID: {id})";

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
            if (eventArgs.Session.Content is GuaranteeDialogView deleteServiceItemDialogView && deleteServiceItemDialogView.DataContext is GuaranteeDialogViewModel model)
            {

                eventArgs.Session.UpdateContent(new ProgressDialog());

                Guarantee updatedDetails = new Guarantee();

                if (model.CustomerBeforeEdit?.CustomerDetails == model.CustomerName)
                {
                    updatedDetails.Customer = model.CustomerBeforeEdit;
                }
                else
                {
                    Customer customer = (await _customerService.GetAll()).FirstOrDefault(c => c.CustomerDetails == model.CustomerName);
                    updatedDetails.Customer = customer;
                }

                updatedDetails.ProductDesc = model.Product;
                updatedDetails.ProductNotes = model.Notes;
                updatedDetails.ProductIMEI = model.IMEI;
                updatedDetails.StartDate = model.StartDate;
                updatedDetails.EndDate = model.EndDate;

                await _guaranteeDataService.Update(model.UpdateID, updatedDetails);

                await LoadGuaranties();
            }
            else
            {
                // show error
            }

            await LoadGuaranties().ContinueWith((t, _) => eventArgs.Session.Close(false), null, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion


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

        #region ShowCustomerDetails
        public ICommand CustomerDetailsCommand => new DialogCommand(ShowCustomerDetailsDialog);
        private async void ShowCustomerDetailsDialog(object idObject)
        {
            int id = (int)idObject;

            Guarantee guarantee = await _guaranteeDataService.Get(id);

            if (guarantee?.Customer == null)
            {
                MessageBox.Show("Η ΕΙΣΑΓΩΓΗ ΤΟΥ ΣΥΓΚΕΚΡΙΜΕΝΟΥ SERVICE ΔΕΝ ΑΝΤΙΣΤΟΙΧΕΙ ΣΕ ΠΕΛΑΤΗ", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);   
                return;
            }

            ShowCustomerDialogView view = new ShowCustomerDialogView();
            ShowCustomerDialogViewModel viewModel = new ShowCustomerDialogViewModel(guarantee.Customer);

            view.DataContext = viewModel;

            //show the dialog
            object result = await DialogHost.Show(view, "RootDialog", OnShowCustomerDetailsDialogOpen, OnShowCustomerDetailsDialogClose);
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void OnShowCustomerDetailsDialogOpen(object sender, DialogOpenedEventArgs eventargs)
        {

        }
        private void OnShowCustomerDetailsDialogClose(object sender, DialogClosingEventArgs eventArgs)
        {

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
                if (ShowExpired)
                {
                    GuaranteeEntries = 
                        (await _guaranteeDataService.GetAll())
                        .OrderByDescending(r => r.ID)
                        .Where
                        (
                            i => i.VirtualID == id || i.ProductIMEI != null && i.ProductIMEI.ToUpper().Contains(value.ToUpper())
                        )
                        .ToList();
                }
                else
                {
                    GuaranteeEntries =
                        (await _guaranteeDataService.GetAll())
                        .OrderByDescending(r => r.ID)
                        .Where
                        (
                            i => i.EndDate > DateTime.Now && i.VirtualID == id || i.ProductIMEI != null && i.ProductIMEI.ToUpper().Contains(value.ToUpper())
                        )
                        .ToList();
                }
            }
            else
            {
                if (ShowExpired)
                {
                    GuaranteeEntries = 
                        (await _guaranteeDataService.GetAll())
                        .Where(c => 
                            c.ProductDesc.ToUpper().Contains(value.ToUpper()) ||
                            c.ProductIMEI != null && c.ProductIMEI.ToUpper().Contains(value.ToUpper()) ||
                            c.CustomerDetails != null && c.CustomerDetails.ToUpper().Contains(value.ToUpper())
                        )
                        .OrderByDescending(r => r.ID)
                        .ToList();
                }
                else
                {
                    GuaranteeEntries =
                        (await _guaranteeDataService.GetAll())
                        .Where(c =>
                            c.EndDate > DateTime.Now &&
                            c.ProductDesc.ToUpper().Contains(value.ToUpper()) ||
                            c.ProductIMEI != null && c.ProductIMEI.ToUpper().Contains(value.ToUpper()) ||
                            c.CustomerDetails != null && c.CustomerDetails.ToUpper().Contains(value.ToUpper())
                        )
                        .OrderByDescending(r => r.ID)
                        .ToList();
                }
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
            if(ShowExpired) GuaranteeEntries = (await _guaranteeDataService.GetAll()).OrderByDescending(r => r.ID).ToList();
            else GuaranteeEntries = (await _guaranteeDataService.GetAll()).OrderByDescending(r => r.ID).Where(it => it.EndDate > DateTime.Now).ToList();
        }
        #endregion
    }
}