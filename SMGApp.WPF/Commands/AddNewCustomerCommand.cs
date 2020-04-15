using System;
using System.Windows;
using System.Windows.Input;
using SMGApp.Domain.Models;
using SMGApp.EntityFramework;
using SMGApp.EntityFramework.Services;
using SMGApp.WPF.States.Navigators;
using SMGApp.WPF.ViewModels;
using SMGApp.WPF.Views;

namespace SMGApp.WPF.Commands
{
    public class AddNewCustomerCommand : ICommand
    {
        private CustomerAddNewView _addNewCustomer;
        public event EventHandler CanExecuteChanged;

        public AddNewCustomerCommand()
        {
            _addNewCustomer = new CustomerAddNewView {WindowStartupLocation = WindowStartupLocation.CenterScreen};
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(_addNewCustomer.WasClosed) _addNewCustomer = new CustomerAddNewView() { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            _addNewCustomer.Show();
            _addNewCustomer.Activate();
        }
    }
}
