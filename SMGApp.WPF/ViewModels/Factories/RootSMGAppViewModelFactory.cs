using System;
using System.Collections.Generic;
using System.Text;
using SMGApp.Domain.Models;
using SMGApp.EntityFramework;
using SMGApp.EntityFramework.Services;
using SMGApp.WPF.States.Navigators;

namespace SMGApp.WPF.ViewModels.Factories
{
    public class RootSMGAppViewModelFactory : IRootSMGAppViewModelAbstractFactory
    {
        private readonly ISMGAppViewModelFactory<CustomerViewModel> _customerViewModelFactory;
        private readonly ISMGAppViewModelFactory<ServiceViewModel> _serviceViewModelFactory;
        private readonly ISMGAppViewModelFactory<InventoryViewModel> _inventoryViewModelFactory;
        private readonly ISMGAppViewModelFactory<BackupViewModel> _backupViewModelFactory;

        public RootSMGAppViewModelFactory(
            ISMGAppViewModelFactory<CustomerViewModel> customerViewModelFactory, 
            ISMGAppViewModelFactory<ServiceViewModel> serviceViewModelFactory, 
            ISMGAppViewModelFactory<InventoryViewModel> inventoryViewModelFactory, 
            ISMGAppViewModelFactory<BackupViewModel> backupViewModelFactory)
        {
            _customerViewModelFactory = customerViewModelFactory;
            _serviceViewModelFactory = serviceViewModelFactory;
            _inventoryViewModelFactory = inventoryViewModelFactory;
            _backupViewModelFactory = backupViewModelFactory;
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
                default:
                    throw new ArgumentException("The View Type does not a ViewModel.", nameof(viewType));
            }
        }
    }
}
