using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using SMGApp.Domain.Models;
using SMGApp.WPF.ViewModels.Util;

namespace SMGApp.WPF.Dialogs.GuaranteeDialogs
{
    public class GuaranteeDialogViewModel : INotifyPropertyChanged
    {
        // Operation Name
        public string OperationName { get; set; }
        public int UpdateID { get; set; } = -1;
        public Customer CustomerBeforeEdit { get; set; }

        public bool EnableState => UpdateID == -1;

        #region ComboBoxCustomers
        private ObservableCollection<string> _comboBoxBoundCustomers = new ObservableCollection<string>();
        public IEnumerable<string> AllCustomers { get; set; }
        public ObservableCollection<string> ComboBoxBoundCustomers
        {
            get => _comboBoxBoundCustomers;
            set => this.MutateVerbose(ref _comboBoxBoundCustomers, value, RaisePropertyChanged());
        }

        public GuaranteeDialogViewModel(List<string> customerNames)
        {
            AllCustomers = customerNames;
            ComboBoxBoundCustomers = new ObservableCollection<string>(customerNames);
        }

        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (_customerName == value) return;

                if (value == null || string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    ComboBoxBoundCustomers = new ObservableCollection<string>(AllCustomers);
                    return;
                }

                _customerName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomerName)));
                IEnumerable<string> filteredCustomers = AllCustomers.Where(it => it.ToUpperΝοintonation().Contains(CustomerName.ToUpperΝοintonation()));
                ComboBoxBoundCustomers = new ObservableCollection<string>(filteredCustomers);
            }
        }
        #endregion

        private string _product;
        private GuaranteeType _guaranteeType;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _notes;
        // ReSharper disable once InconsistentNaming
        private string _IMEI;

        public string Product
        {
            get => _product;
            set => this.MutateVerbose(ref _product, value, RaisePropertyChanged());
        }

        public string Notes
        {
            get => _notes;
            set => this.MutateVerbose(ref _notes, value, RaisePropertyChanged());
        }

        public string IMEI
        {
            get => _IMEI;
            set => this.MutateVerbose(ref _IMEI, value, RaisePropertyChanged());
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => this.MutateVerbose(ref _startDate, value, RaisePropertyChanged());

        }

        public DateTime EndDate
        {
            get => _endDate;
            set => this.MutateVerbose(ref _endDate, value, RaisePropertyChanged());

        }

        public GuaranteeType GuaranteeType
        {
            get => _guaranteeType;
            set => this.MutateVerbose(ref _guaranteeType, value, RaisePropertyChanged());
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged() => args => PropertyChanged?.Invoke(this, args);
    }
}
