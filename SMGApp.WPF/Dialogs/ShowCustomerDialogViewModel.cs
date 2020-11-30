using System;
using System.ComponentModel;
using SMGApp.Domain.Models;

namespace SMGApp.WPF.Dialogs
{
    public class ShowCustomerDialogViewModel : INotifyPropertyChanged
    {
        public ShowCustomerDialogViewModel(Customer customer)
        {
            if (customer == null) return;
            
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Address = customer.Address;
            PhoneNumber = customer.PhoneNumber;
            Note = customer.Notes;
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => this.MutateVerbose(ref _firstName, value, RaisePropertyChanged());
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => this.MutateVerbose(ref _lastName, value, RaisePropertyChanged());
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => this.MutateVerbose(ref _address, value, RaisePropertyChanged());
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => this.MutateVerbose(ref _phoneNumber, value, RaisePropertyChanged());
        }

        private string _note;
        public string Note
        {
            get => _note;
            set => this.MutateVerbose(ref _note, value, RaisePropertyChanged());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged() => args => PropertyChanged?.Invoke(this, args);
    }
}
