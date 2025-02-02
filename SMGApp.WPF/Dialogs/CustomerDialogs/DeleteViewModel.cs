﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using SMGApp.Domain.Models;

namespace SMGApp.WPF.Dialogs.CustomerDialogs
{
    public class DeleteViewModel : INotifyPropertyChanged
    {
        public int CustomerID { get; set; }
        public IEnumerable<ServiceItem> ServiceEntries { get; set; }

        public string DeleteCustomerName { get; set; }
        public string DeleteCustomerServiceEntries { get; set; }
        public string DeleteCustomerWarning { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged() => args => PropertyChanged?.Invoke(this, args);
    }
}
