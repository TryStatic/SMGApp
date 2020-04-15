using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SMGApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for CustomerAddNew.xaml
    /// </summary>
    public partial class CustomerAddNewView : Window
    {
        public CustomerAddNewView()
        {
            InitializeComponent();
        }

        public bool WasClosed { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            WasClosed = true;
        }
    }
}
