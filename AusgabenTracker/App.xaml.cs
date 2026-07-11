using AusgabenTracker.Data;
using AusgabenTracker.ViewModels;
using AusgabenTracker.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AusgabenTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new();

            string connectionString = "Server=DESKTOP-TEIN2QD;Database=Ausgaben_Tracker;Trusted_Connection=True;TrustServerCertificate=True;";

            services.AddSingleton(new DBHelper(connectionString));
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = _serviceProvider.GetRequiredService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }
    }

}
