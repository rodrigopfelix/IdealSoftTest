using IdealSoftTestWPFClient.Services;
using IdealSoftTestWPFClient.ViewModels.Customers;
using IdealSoftTestWPFClient.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace IdealSoftTestWPFClient
{
    public partial class App : Application
    {
        private IHost _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    // HttpClient + API
                    var baseAddress = new Uri("http://localhost:5219/");
                    services.AddHttpClient<ICustomerService, CustomerApiService>(c => { c.BaseAddress = baseAddress; });
                    services.AddHttpClient<IPhoneService, PhoneApiService>(p => { p.BaseAddress = baseAddress; });

                    // ViewModels
                    services.AddSingleton<CustomersViewModel>();
                    services.AddSingleton<CustomerPhoneEditorViewModel>();
                    services.AddTransient<CustomerEditorViewModel>();
                })
                .Build();

            // Janela principal
            var mainWindow = new CustomersView
            {
                DataContext = _host.Services
                    .GetRequiredService<CustomersViewModel>()
            };

            MainWindow = mainWindow;
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
                await _host.StopAsync();

            base.OnExit(e);
        }
    }
}
