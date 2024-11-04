using System.Windows;
using GardenGroup.StartupHelpers;
using GardenGroup.ViewModels;
using GardenGroup.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service;

namespace GardenGroup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<IViewModelFactory, ViewModelFactory>();

                    // Register all view models
                    // Add new viewmodels here
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<PasswordResetViewModel>();
                    services.AddTransient<DashboardViewModel>();
                    services.AddTransient<TicketViewModel>();
                    services.AddTransient<EmployeeTicketsViewModel>();
                    services.AddTransient<ManageEmployeesViewModel>();



                    services.AddTransient<IServiceManager, ServiceManager>();
                }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }

}