using System.Windows.Controls;
using System.Windows.Input;

namespace SMGApp.WPF.Dialogs.GuaranteeDialogs
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class GuaranteeDialogView : UserControl
    {
        public GuaranteeDialogView()
        {
            InitializeComponent();
        }

        private void Cb_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Cb.IsDropDownOpen = true;
        }
    }
}
