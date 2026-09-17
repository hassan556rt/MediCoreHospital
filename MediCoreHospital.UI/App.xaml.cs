using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Infrastructure.Configuration;
using MediCoreHospital.Infrastructure.Data;
using MediCoreHospital.Infrastructure.Services;
using MediCoreHospital.UI.ViewModels.Appointments;
using MediCoreHospital.UI.ViewModels.Billing;
using MediCoreHospital.UI.ViewModels.Clinical;
using MediCoreHospital.UI.ViewModels.Dashboard;
using MediCoreHospital.UI.ViewModels.Inpatient;
using MediCoreHospital.UI.ViewModels.Patients;
using MediCoreHospital.UI.ViewModels.Pharmacy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MediCoreHospital.UI;

public partial class App : System.Windows.Application
{
    public IServiceProvider Services { get; private set; } = null!;

    private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<AppConfiguration>();
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IClinicalDirectoryService, ClinicalDirectoryService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IInpatientService, InpatientService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IPharmacyService, PharmacyService>();

        services.AddTransient<DashboardViewModel>();
        services.AddTransient<PatientsViewModel>();
        services.AddTransient<ClinicalDirectoryViewModel>();
        services.AddTransient<AppointmentsViewModel>();
        services.AddTransient<InpatientViewModel>();
        services.AddTransient<BillingViewModel>();
        services.AddTransient<PharmacyViewModel>();
        services.AddTransient<MainWindow>();

        Services = services.BuildServiceProvider();
        Services.GetRequiredService<MainWindow>().Show();
    }
}
