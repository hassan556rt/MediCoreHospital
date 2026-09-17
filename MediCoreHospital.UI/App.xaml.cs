using System.Windows;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Infrastructure.Configuration;
using MediCoreHospital.Infrastructure.Data;
using MediCoreHospital.Infrastructure.Services;
using MediCoreHospital.UI.ViewModels.Dashboard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MediCoreHospital.UI;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; } = null!;

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<AppConfiguration>();
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<MainWindow>();

        Services = services.BuildServiceProvider();
        Services.GetRequiredService<MainWindow>().Show();
    }
}
