using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Infrastructure.Configuration;
using MediCoreHospital.Infrastructure.Data;
using MediCoreHospital.Infrastructure.Services;
using MediCoreHospital.UI.ViewModels.Dashboard;
using MediCoreHospital.UI.ViewModels.Patients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MediCoreHospital.UI;

public partial class App : System.Windows.Application
{
    public IServiceProvider Services { get; private set; } = null!;
    private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
    {
        var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", false, true).Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration).AddSingleton<AppConfiguration>().AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddTransient<DashboardViewModel>().AddTransient<PatientsViewModel>().AddTransient<MainWindow>();
        Services = services.BuildServiceProvider();
        Services.GetRequiredService<MainWindow>().Show();
    }
}
