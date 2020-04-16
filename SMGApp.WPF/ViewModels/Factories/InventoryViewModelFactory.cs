namespace SMGApp.WPF.ViewModels.Factories
{
    public class InventoryViewModelFactory : ISMGAppViewModelFactory<InventoryViewModel>
    {
        public InventoryViewModel CreateViewModel()
        {
            return new InventoryViewModel();
        }
    }
}