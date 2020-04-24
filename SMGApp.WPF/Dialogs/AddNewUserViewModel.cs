using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SMGApp.WPF.Dialogs
{
    public class AddNewUserViewModel : INotifyPropertyChanged
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

    public static class NotifyPropertyChangedExtension
    {
        public static void MutateVerbose<TField>(this INotifyPropertyChanged instance, ref TField field, TField newValue, Action<PropertyChangedEventArgs> raise, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, newValue)) return;
            field = newValue;
            raise?.Invoke(new PropertyChangedEventArgs(propertyName));
        }
    }
}
