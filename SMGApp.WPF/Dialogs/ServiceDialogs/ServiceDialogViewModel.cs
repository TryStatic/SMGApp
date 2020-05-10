using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SMGApp.WPF.Dialogs.ServiceDialogs
{
    public class ServiceDialogViewModel : INotifyPropertyChanged
    {
        // Operation Name
        public string OperationName { get; set; }
        public int UpdateID { get; set; }
        public DateTime UpdateDateAdded { get; set; }

        #region ComboBoxCustomers
        private ObservableCollection<string> _comboBoxBoundCustomers = new ObservableCollection<string>();
        public IEnumerable<string> AllCustomers { get; set; }
        public ObservableCollection<string> ComboBoxBoundCustomers
        {
            get => _comboBoxBoundCustomers;
            set => this.MutateVerbose(ref _comboBoxBoundCustomers, value, RaisePropertyChanged());
        }

        public ServiceDialogViewModel(List<string> customerNames)
        {
            AllCustomers = customerNames;
            ComboBoxBoundCustomers = new ObservableCollection<string>(customerNames);
        }

        private string _searchTextText;
        public string SearchTextText
        {
            get => _searchTextText;
            set
            {
                if (_searchTextText == value) return;
                _searchTextText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchTextText)));

                IEnumerable<string> enumerable = AllCustomers.Where(it => it.Contains(SearchTextText));
                ComboBoxBoundCustomers = new ObservableCollection<string>(enumerable);
            }
        }
        #endregion



        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged() => args => PropertyChanged?.Invoke(this, args);
    }
}
