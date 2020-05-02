using System;
using System.Collections.Generic;
using System.ComponentModel;
using SMGApp.Domain.Models;

namespace SMGApp.WPF.Dialogs.ServiceDialogs
{
    public class DeleteServiceItemDialogViewModel : INotifyPropertyChanged
    {
        public int ServiceEntryID { get; set; }
        public string ProductName { get; set; }
        public string RelatedCustomer { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged() => args => PropertyChanged?.Invoke(this, args);
    }
}
