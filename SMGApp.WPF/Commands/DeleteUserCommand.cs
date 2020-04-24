using System;
using System.Windows;
using System.Windows.Input;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;
using SMGApp.WPF.ViewModels;

namespace SMGApp.WPF.Commands
{
    public class DeleteUserCommand : ICommand
    {
        private readonly IDataService<Customer> customerDataService;
        private readonly CustomerViewModel _customerViewModel;

        public DeleteUserCommand(IDataService<Customer> customerDataService, ViewModels.CustomerViewModel customerViewModel)
        {
            this.customerDataService = customerDataService;
            _customerViewModel = customerViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(!(parameter is int)) return;
            if((int)parameter < 0) return;
            await customerDataService.Delete((int)parameter);
            await _customerViewModel.LoadCustomers();
        }

        public event EventHandler CanExecuteChanged;
    }
}