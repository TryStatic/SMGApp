using System.Windows;
using SMGApp.EntityFramework;
using SMGApp.WPF.ViewModels;

namespace SMGApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {

            await SMGAppDbContextFactory.MigrateIfNeeded();

            Window window = new MainWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Height = 720;
            window.Width = 1280;
            window.DataContext = new MainViewModel();
            window.Show();

            base.OnStartup(e);
        }
    }
}
