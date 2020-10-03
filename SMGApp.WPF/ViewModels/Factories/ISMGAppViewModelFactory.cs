namespace SMGApp.WPF.ViewModels.Factories
{
    public interface ISMGAppViewModelFactory<T> where T : ViewModelBase
    {
        T CreateViewModel();
    }
}
