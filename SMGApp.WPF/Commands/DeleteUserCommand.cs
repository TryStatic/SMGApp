using System;
using System.Windows;
using System.Windows.Input;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;

namespace SMGApp.WPF.Commands
{
    public class DeleteUserCommand : ICommand
    {
        private readonly IDataService<Customer> customerDataService;

        public DeleteUserCommand(IDataService<Customer> customerDataService)
        {
            this.customerDataService = customerDataService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(!(parameter is int)) return;
            if((int)parameter < 0) return;
            customerDataService.Delete((int)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}