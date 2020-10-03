using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
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
        public static CheckBoxStates CheckBoxesStates = new CheckBoxStates();

        protected override async void OnStartup(StartupEventArgs e)
        {
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);

            if (processes.Length > 1)
            {
                MessageBox.Show($"{procName} already running", "Error");
                Current.Shutdown();
                return;
            }

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\receipts"))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\receipts");
                }
                catch (Exception)
                {
                    MessageBox.Show("Error creating receipts directory.");
                    Environment.Exit(1);
                }

            }


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
            services.AddSingleton<GuaranteeDataService, GuaranteeDataService>();

            services.AddSingleton<IRootSMGAppViewModelAbstractFactory, RootSMGAppViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<CustomerViewModel>, CustomerViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<ServiceViewModel>, ServiceViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<InventoryViewModel>, InventoryViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<BackupViewModel>, BackupViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<GuaranteeViewModel>, GuaranteeViewModelFactory>();
            services.AddSingleton<ISMGAppViewModelFactory<InvoiceViewModel>, InvoiceViewModelFactory>();

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

        public class CheckBoxStates
        {
            public bool ServiceArrived { get; set; } = true;
            public bool ServiceFixed { get; set; } = true;
            public bool ServiceDelivered { get; set; } = true;
            public bool ServiceIssue { get; set; } = true;
            public bool ServiceHide { get; set; } = true;
            public bool GuaranteeHide { get; set; } = true;
        }
    }
}
