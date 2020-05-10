using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;
using SMGApp.EntityFramework;
using SMGApp.EntityFramework.Services;
using SMGApp.WPF.States.Navigators;
using SMGApp.WPF.ViewModels;
using SMGApp.WPF.ViewModels.Factories;

namespace SMGApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("el-GR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("el-GR");
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement), 
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));


            await SMGAppDbContextFactory.MigrateIfNeeded();

            IServiceProvider serviceProvider = CreateServiceProvider();

            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Height = SystemParameters.PrimaryScreenHeight * 0.95;
            window.Width = SystemParameters.PrimaryScreenWidth * 0.95;
            window.DataContext = serviceProvider.GetRequiredService<MainViewModel>();
            window.Show();

            base.OnStartup(e);
        }

        private static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<SMGAppDbContextFactory>();
            services.AddSingleton<CustomersDataService, CustomersDataService>();
            services.AddSingleton<ServiceItemsDataService, ServiceItemsDataService>();

            services.AddSingleton<IRootSMGAppViewModelAbstractFactory, RootSMGAppViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<CustomerViewModel>, CustomerViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<ServiceViewModel>, ServiceViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<InventoryViewModel>, InventoryViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<BackupViewModel>, BackupViewModelFactory>();

            services.AddScoped<INavigator, Navigator>();

            services.AddScoped<MainViewModel>();

            services.AddScoped<MainWindow>(provider => new MainWindow(provider.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("error.log", true))
            {
                writer.WriteLine($"--------------[{DateTime.Now}]--------------");
                writer.WriteLine(e.Exception);
                writer.Write("\n\n");
            }
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}
