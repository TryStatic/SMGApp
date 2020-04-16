using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
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
            window.Height = 800;
            window.Width = 1280;
            window.DataContext = serviceProvider.GetRequiredService<MainViewModel>();
            window.Show();

            base.OnStartup(e);
        }

        private static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<SMGAppDbContextFactory>();
            services.AddSingleton<IDataService<Customer>, GenericDataServices<Customer>>();

            services.AddSingleton<ISMGAppViewModelAbstractFactory, SMGAppViewModelAbstractFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<CustomerViewModel>, CustomerViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<ServiceViewModel>, ServiceViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<InventoryViewModel>, InventoryViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<BackupViewModel>, BackupViewModelFactory>();

            services.AddScoped<INavigator, Navigator>();

            services.AddScoped<MainViewModel>();

            services.AddScoped<MainWindow>(provider => new MainWindow(provider.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }
}
