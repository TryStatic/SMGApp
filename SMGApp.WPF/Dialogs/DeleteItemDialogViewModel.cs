using System;
using System.ComponentModel;

namespace SMGApp.WPF.Dialogs
{
    public class DeleteItemDialogViewModel : INotifyPropertyChanged
    {
        public int ServiceEntryID { get; set; }
        public string ProductName { get; set; }
        public string RelatedCustomer { get; set; }
        public string RelatedTab { get; set; } = "";

        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged() => args => PropertyChanged?.Invoke(this, args);
    }
}
