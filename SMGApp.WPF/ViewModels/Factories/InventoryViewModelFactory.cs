namespace SMGApp.WPF.ViewModels.Factories
{
    public class InventoryViewModelFactory : ISMGAppViewModelFactory<InventoryViewModel>
    {
        public InventoryViewModel CreateViewModel()
        {
            return new InventoryViewModel();
        }
    }

    public class InvoiceViewModelFactory : ISMGAppViewModelFactory<InvoiceViewModel>
    {
        public InvoiceViewModel CreateViewModel()
        {
            return new InvoiceViewModel();
        }
    }
}