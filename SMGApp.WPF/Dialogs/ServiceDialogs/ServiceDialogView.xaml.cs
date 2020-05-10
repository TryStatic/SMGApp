using System.Windows.Controls;
using System.Windows.Input;

namespace SMGApp.WPF.Dialogs.ServiceDialogs
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class ServiceDialogView : UserControl
    {
        public ServiceDialogView()
        {
            InitializeComponent();
        }

        private void Cb_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Cb.IsDropDownOpen = true;
        }
    }
}
