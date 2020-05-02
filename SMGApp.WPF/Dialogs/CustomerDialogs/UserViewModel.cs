using System;
using System.ComponentModel;

namespace SMGApp.WPF.Dialogs.CustomerDialogs
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public string OperationName { get; set; }

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

        public int UpdateID { get; set; }
        public DateTime UpdateDateAdded { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged() => args => PropertyChanged?.Invoke(this, args);
    }
}
