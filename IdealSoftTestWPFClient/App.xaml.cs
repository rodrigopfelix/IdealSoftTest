using IdealSoftTestWPFClient.Models;
using IdealSoftTestWPFClient.Services;
using IdealSoftTestWPFClient.ViewModels;
using IdealSoftTestWPFClient.ViewModels.Customers;
using IdealSoftTestWPFClient.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
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
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    IConfiguration config = context.Configuration;
                    services.Configure<ApiSettings>(config.GetSection("Api"));


                    services.AddHttpClient("ApiClient", client =>
                                {
                                    client.BaseAddress = new Uri(config["Api:Url"]);
                                })
                            .AddHttpMessageHandler<AuthHttpMessageHandler>();
                    
                    services.AddScoped(sp =>
                        sp.GetRequiredService<IHttpClientFactory>()
                          .CreateClient("ApiClient"));

                    services.AddScoped<IAuthService, AuthService>();
                    services.AddScoped<ICustomerService, CustomerApiService>();
                    services.AddScoped<IPhoneService, PhoneApiService>();

                    services.AddSingleton<CustomersViewModel>();
                    services.AddSingleton<CustomerPhoneEditorViewModel>();
                    services.AddSingleton<ITokenStore, InMemoryTokenStore>();

                    services.AddTransient<CustomerEditorViewModel>();
                    services.AddTransient<AuthHttpMessageHandler>();
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
