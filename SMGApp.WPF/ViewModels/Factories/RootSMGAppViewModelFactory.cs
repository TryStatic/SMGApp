using System;
using SMGApp.WPF.States.Navigators;

namespace SMGApp.WPF.ViewModels.Factories
{
    public class RootSMGAppViewModelFactory : IRootSMGAppViewModelAbstractFactory
    {
        private readonly ISMGAppViewModelFactory<CustomerViewModel> _customerViewModelFactory;
        private readonly ISMGAppViewModelFactory<ServiceViewModel> _serviceViewModelFactory;
        private readonly ISMGAppViewModelFactory<InventoryViewModel> _inventoryViewModelFactory;
        private readonly ISMGAppViewModelFactory<BackupViewModel> _backupViewModelFactory;
        private readonly ISMGAppViewModelFactory<GuaranteeViewModel> _guaranteeViewModelFactory;
        private readonly ISMGAppViewModelFactory<InvoiceViewModel> _invoiceViewModelFactory;

        public RootSMGAppViewModelFactory(
            ISMGAppViewModelFactory<CustomerViewModel> customerViewModelFactory, 
            ISMGAppViewModelFactory<ServiceViewModel> serviceViewModelFactory, 
            ISMGAppViewModelFactory<InventoryViewModel> inventoryViewModelFactory,
            ISMGAppViewModelFactory<BackupViewModel> backupViewModelFactory,
            ISMGAppViewModelFactory<GuaranteeViewModel> guaranteeViewModelFactory, 
            ISMGAppViewModelFactory<InvoiceViewModel> invoiceViewModelFactory)
        {
            _customerViewModelFactory = customerViewModelFactory;
            _serviceViewModelFactory = serviceViewModelFactory;
            _inventoryViewModelFactory = inventoryViewModelFactory;
            _backupViewModelFactory = backupViewModelFactory;
            _guaranteeViewModelFactory = guaranteeViewModelFactory;
            _invoiceViewModelFactory = invoiceViewModelFactory;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {

            switch (viewType)
            {
                case ViewType.Customer:
                    return _customerViewModelFactory.CreateViewModel();
                case ViewType.Service:
                    return _serviceViewModelFactory.CreateViewModel();
                case ViewType.Inventory:
                    return _inventoryViewModelFactory.CreateViewModel();
                case ViewType.Backup:
                    return _backupViewModelFactory.CreateViewModel();
                case ViewType.Guarantee:
                    return _guaranteeViewModelFactory.CreateViewModel();
                case ViewType.Invoice:
                    return _invoiceViewModelFactory.CreateViewModel();
                default:
                    throw new ArgumentException("The View Type does not a ViewModel.", nameof(viewType));
            }
        }
    }
}
