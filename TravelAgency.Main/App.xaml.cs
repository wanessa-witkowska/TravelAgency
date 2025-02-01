using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Reflection;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Services;
using TravelAgency.ViewModels;
using System.Configuration;

namespace TravelAgency.Main
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); // Ensure base startup logic is called

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                // Sprawdź nazwę załadowanej assembly
                if (args.Name.Contains("travelAgency.Controls"))
                {
                    // Ścieżka do pliku DLL
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "travelAgency.Controls.dll");
                    if (File.Exists(path))
                    {
                        return Assembly.LoadFrom(path);
                    }
                }
                return null;
            };

            // Subscribe to the unhandled exception event
            DispatcherUnhandledException += ApplicationDispatcherUnhandledException;

            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Load data from JSON file if it exists
            var dbContext = ServiceProvider.GetService<travelAgencyContext>();
            if (dbContext != null)
            {
                dbContext.LoadData("data.json");
            }

            MainWindow mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();

            // Register the OnProcessExit event handler
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<travelAgencyContext>(options =>
            {
                options.UseInMemoryDatabase("travelAgencyDb");
                options.UseLazyLoadingProxies();
            });
            serviceCollection.AddSingleton<IDialogService, DialogService>();
            serviceCollection.AddSingleton<IBookingService, BookingService>();
            serviceCollection.AddSingleton<ICustomerService, CustomerService>();
            serviceCollection.AddSingleton<IGuideService, GuideService>();
            serviceCollection.AddSingleton<ILocationService, LocationService>();
            serviceCollection.AddSingleton<ITourService, TourService>();

            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddSingleton<travelAgencyContext>();
        }

        private void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                "An unhandled exception just occurred: " + e.Exception.Message,
                "Exception",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
            e.Handled = true;
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            var dbContext = ServiceProvider.GetService<travelAgencyContext>();
            if (dbContext != null)
            {
                // Save data to JSON file on application exit
                dbContext.SaveData("data.json");
            }
        }
    }
}
