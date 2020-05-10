using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using SMGApp.Domain.Models;
using SMGApp.WPF.ViewModels.Util;

namespace SMGApp.WPF.Dialogs.ServiceDialogs
{
    public class ServiceDialogViewModel : INotifyPropertyChanged
    {
        // Operation Name
        public string OperationName { get; set; }
        public int UpdateID { get; set; }
        public Customer CustomerBeforeEdit { get; set; }
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

        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (_customerName == value || value == null) return;
                _customerName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomerName)));
                if (_customerName != null)
                {
                    IEnumerable<string> enumerable = AllCustomers.Where(it => it.ToUpperΝοintonation().Contains(CustomerName.ToUpperΝοintonation()));
                    ComboBoxBoundCustomers = new ObservableCollection<string>(enumerable);
                }

            }
        }
        #endregion

        private string _device;
        private string _damageDescription;
        private string _notes;
        private string _devicePassword;
        private string _simCode;
        private string _accountUsername;
        private string _accountPassword;
        private bool _chargerIncluded;
        private bool _bagIncluded;
        private bool _caseIncluded;
        private ServiceState _serviceState;
        private double _cost;

        public string Device
        {
            get => _device;
            set => this.MutateVerbose(ref _device, value, RaisePropertyChanged());
        }

        public string DamageDescription
        {
            get => _damageDescription;
            set => this.MutateVerbose(ref _damageDescription, value, RaisePropertyChanged());

        }

        public string Notes
        {
            get => _notes;
            set => this.MutateVerbose(ref _notes, value, RaisePropertyChanged());
        }

        public string DevicePassword
        {
            get => _devicePassword;
            set => this.MutateVerbose(ref _devicePassword, value, RaisePropertyChanged());

        }

        public string SimCode
        {
            get => _simCode;
            set => this.MutateVerbose(ref _simCode, value, RaisePropertyChanged());
        }

        public string AccountUsername
        {
            get => _accountUsername;
            set => this.MutateVerbose(ref _accountUsername, value, RaisePropertyChanged());

        }

        public string AccountPassword
        {
            get => _accountPassword;
            set => this.MutateVerbose(ref _accountPassword, value, RaisePropertyChanged());
        }

        public bool ChargerIncluded
        {
            get => _chargerIncluded;
            set => this.MutateVerbose(ref _chargerIncluded, value, RaisePropertyChanged());

        }

        public bool BagIncluded
        {
            get => _bagIncluded;
            set => this.MutateVerbose(ref _bagIncluded, value, RaisePropertyChanged());

        }

        public bool CaseIncluded
        {
            get => _caseIncluded;
            set => this.MutateVerbose(ref _caseIncluded, value, RaisePropertyChanged());

        }

        public ServiceState ServiceState
        {
            get => _serviceState;
            set => this.MutateVerbose(ref _serviceState, value, RaisePropertyChanged());
        }

        public double Cost
        {
            get => _cost;
            set => this.MutateVerbose(ref _cost, value, RaisePropertyChanged());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged() => args => PropertyChanged?.Invoke(this, args);
    }
}
