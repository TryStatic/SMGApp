using System;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using SMGApp.Domain.Models;
using SMGApp.EntityFramework;
using SMGApp.EntityFramework.Services;
using SMGApp.WPF.States.Navigators;
using SMGApp.WPF.ViewModels;

namespace SMGApp.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!(parameter is ViewType viewType)) return;
            
            switch (viewType)
            {
                case ViewType.Customer:
                    _navigator.CurrentViewModel = new CustomerViewModel(new GenericDataServices<Customer>(new SMGAppDbContextFactory()));
                    break;
                case ViewType.Service:
                    _navigator.CurrentViewModel = new ServiceViewModel();
                    break;
                case ViewType.Inventory:
                    _navigator.CurrentViewModel = new InventoryViewModel();
                    break;
                case ViewType.Backup:
                    _navigator.CurrentViewModel = new BackupViewModel();
                    break;
                default:
                    break;
            }
        }

    }
}
