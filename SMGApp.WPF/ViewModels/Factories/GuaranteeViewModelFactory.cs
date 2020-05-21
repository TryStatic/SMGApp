namespace SMGApp.WPF.ViewModels.Factories
{
    public class GuaranteeViewModelFactory : ISMGAppViewModelFactory<GuaranteeViewModel>
    {
        public GuaranteeViewModel CreateViewModel()
        {
            return new GuaranteeViewModel();
        }
    }
}