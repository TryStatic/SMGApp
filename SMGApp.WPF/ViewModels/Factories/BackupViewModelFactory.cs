namespace SMGApp.WPF.ViewModels.Factories
{
    public class BackupViewModelFactory : ISMGAppViewModelFactory<BackupViewModel>
    {
        public BackupViewModel CreateViewModel()
        {
            return new BackupViewModel();
        }
    }
}