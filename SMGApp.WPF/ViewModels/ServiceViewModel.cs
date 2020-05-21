using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using MaterialDesignThemes.Wpf;
using SMGApp.Domain.Models;
using SMGApp.EntityFramework.Services;
using SMGApp.WPF.Commands;
using SMGApp.WPF.Dialogs;
using SMGApp.WPF.Dialogs.ServiceDialogs;
using SMGApp.WPF.ViewModels.Util;
using HorizontalAlignment = iText.Layout.Properties.HorizontalAlignment;
using TextAlignment = iText.Layout.Properties.TextAlignment;

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
                newDetails.DeviceDescription = model.Device.ToUpperΝοintonation();
                newDetails.DamageDescription = model.DamageDescription.ToUpperΝοintonation();
                newDetails.Notes = model.Notes.ToUpperΝοintonation();
                newDetails.DevicePassword = model.DevicePassword.ToUpperΝοintonation();
                newDetails.SimPassword = model.SimCode.ToUpperΝοintonation();
                newDetails.DeviceAccountUsername = model.AccountUsername.ToUpperΝοintonation();
                newDetails.DeviceAccountPassword = model.AccountPassword.ToUpperΝοintonation();
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
                updatedDetails.DeviceDescription = model.Device.ToUpperΝοintonation();
                updatedDetails.DamageDescription = model.DamageDescription.ToUpperΝοintonation();
                updatedDetails.Notes = model.Notes.ToUpperΝοintonation();
                updatedDetails.DevicePassword = model.DevicePassword.ToUpperΝοintonation();
                updatedDetails.SimPassword = model.SimCode.ToUpperΝοintonation();
                updatedDetails.DeviceAccountUsername = model.AccountUsername.ToUpperΝοintonation();
                updatedDetails.DeviceAccountPassword = model.AccountPassword.ToUpperΝοintonation();
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

            DeleteItemDialogView view = new DeleteItemDialogView();
            DeleteItemDialogViewModel viewmodel = new DeleteItemDialogViewModel();

            viewmodel.ProductName = serviceEntry.DeviceDescription;
            viewmodel.ServiceEntryID = serviceEntry.ID;
            viewmodel.RelatedCustomer = serviceEntry.CustomerDetails;
            viewmodel.RelatedTab = "SERVICE";
            
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
        
        public ICommand PrintReceiptCommand => new GenericCommand(async o =>
        {
            int id = (int) o;
            ServiceItem serviceEntry = await _serviceItemsDataService.Get(id);

            if (serviceEntry == null)
            {
                MessageBox.Show("Error Retreving Service Entry " + id);
                return;
            }

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\receipts"))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\receipts");
                }
                catch (Exception)
                {
                    MessageBox.Show("Error creating receipts directory.");
                    //return;
                }

            }

            string path = AppDomain.CurrentDomain.BaseDirectory + $"\\receipts\\receipt{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.pdf";

            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, new PageSize(8.5f * 72.0f, 11.0f * 72.0f));

            document.SetMargins(
                topMargin: 0.39370079f * 72.0f,
                rightMargin: 16.45f * 0.39370079f * 72.0f,
                bottomMargin: 0.39370079f * 72.0f,
                leftMargin: 0.5f * 0.39370079f * 72.0f);


            //PdfFont font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFontFamilies.TIMES, "Identity-H");
            ImageData imageData = ImageDataFactory.Create(WPF.Properties.Resources.smg_blackwhite);
            Image image = new Image(imageData).SetAutoScale(true);
            document.Add(image);

            PdfFont liberationSansBold = PdfFontFactory.CreateFont(Properties.Resources.LiberationSans_Bold, PdfEncodings.IDENTITY_H, true);

            document.Add(new Paragraph("").SetTextAlignment(TextAlignment.CENTER)).SetFont(liberationSansBold).SetBottomMargin(0);

            Text name = new Text("ΚΥΡΙΑΚΟΣ ΣΕΜΕΡΤΖΙΔΗΣ\n").SetFontSize(10.5f);
            Text address = new Text("ΕΘΝΙΚΗΣ ΑΝΤΙΣΤΑΣΗΣ 25\n").SetFontSize(10.5f);
            Text phone = new Text("ΤΗΛ: 2351023223\n").SetFontSize(10.5f);
            Text afm = new Text("ΑΦΜ: 153051582\n").SetFontSize(10.5f);
            Text comment = new Text("ΕΜΠΟΡΙΑ & ΕΠΙΣΚΕΥΕΣ Η/Υ\n").SetFontSize(9.5f);

            Paragraph headerParagraph = new Paragraph().SetFixedLeading(9f).SetMarginTop(0);
            headerParagraph.Add(name);
            headerParagraph.Add(address);
            headerParagraph.Add(phone);
            headerParagraph.Add(afm);
            headerParagraph.Add(comment);

            document.Add(headerParagraph).SetFont(liberationSansBold);

            // Line Separator
            document.Add(new LineSeparator(new SolidLine(1f)).SetMarginBottom(0));

            document.Add(new Paragraph($"ΠΑΡΑΔΟΣΗ ΣΥΣΚΕΥΗΣ\n{DateTime.Now:dd/MM/yyyy HH:mm}").SetFontSize(11f).SetFixedLeading(11.0f).SetTextAlignment(TextAlignment.CENTER)).SetFont(liberationSansBold);

            // Line Separator (Dotted)
            document.Add(new LineSeparator(new DottedLine(1f)).SetMarginBottom(5f));


            Text cName = new Text("ΟΝΟΜΑΤΕΠΩΝΥΜΟ:").SetFontSize(10.0f).SetUnderline();
            Text cNameFilled = new Text($" {serviceEntry.Customer.CustomerDetails}\n\n").SetFontSize(10.5f);

            Text cDevice = new Text("ΣΥΣΚΕΥΗ:").SetFontSize(10.0f).SetUnderline();
            Text cDeviceFilled = new Text($" {serviceEntry.DeviceDescription} (ID: {serviceEntry.ID})\n\n").SetFontSize(10.5f);

            Text cDmg = new Text("ΒΛΑΒΗ:").SetFontSize(10.0f).SetUnderline();
            Text cDmgFilled = new Text($" {serviceEntry.DamageDescription}\n\n").SetFontSize(10.5f);


            Text cAccessories = new Text("ΠΑΡΕΛΚΟΜΕΝΑ:\n").SetFontSize(10.0f).SetUnderline();
            string caseIncluded = serviceEntry.CaseIncluded ? "ΝΑΙ" : "ΟΧΙ";
            string chargerIncluded = serviceEntry.ChargerIncluded ? "ΝΑΙ" : "ΟΧΙ";
            string bagIncluded = serviceEntry.BagIncluded ? "ΝΑΙ" : "ΟΧΙ";
            Text cAccessoriesFilled = new Text($"-ΘΗΚΗ [{caseIncluded}]\n\t-ΦΟΡΤΗΣΤΗΣ [{chargerIncluded}]\n\t-ΤΣΑΝΤΑ [{bagIncluded}]\n\n").SetFontSize(10.5f);

            Text cPrice = new Text("ΚΟΣΤΟΣ:").SetFontSize(10.0f).SetUnderline();
            Text cPriceFilled = new Text($" {serviceEntry.GetCost}\n\n").SetFontSize(10.5f);

            Paragraph customerParagraph = new Paragraph().SetFixedLeading(11f).SetMarginTop(5f);
            customerParagraph.Add(cName);
            customerParagraph.Add(cNameFilled);
            customerParagraph.Add(cDevice);
            customerParagraph.Add(cDeviceFilled);
            customerParagraph.Add(cDmg);
            customerParagraph.Add(cDmgFilled);
            customerParagraph.Add(cAccessories);
            customerParagraph.Add(cAccessoriesFilled);
            customerParagraph.Add(cPrice);
            customerParagraph.Add(cPriceFilled);
            document.Add(customerParagraph).SetFont(liberationSansBold);

            // Line Separator (Dotted)
            document.Add(new LineSeparator(new DottedLine(1f)).SetMarginBottom(5f));
            document.Add(new Paragraph("ΕΥΧΑΡΙΣΤΟΥΜΕ ΓΙΑ ΤΗΝ ΠΡΟΤΙΜΗΣΗ ΣΑΣ").SetFixedLeading(11f).SetMarginTop(5f));


            document.Close();

            Process p = new Process();
            ProcessStartInfo pi = new ProcessStartInfo {UseShellExecute = true, FileName = path};
            p.StartInfo = pi;
            p.Start();

        });
    }


}