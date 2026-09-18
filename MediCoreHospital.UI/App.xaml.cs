using System.Windows;
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
using MediCoreHospital.UI.Views.Login;
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
        services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));

        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<IDashboardService, DashboardService>();
        services.AddSingleton<IPatientService, PatientService>();
        services.AddSingleton<IClinicalDirectoryService, ClinicalDirectoryService>();
        services.AddSingleton<IAppointmentService, AppointmentService>();
        services.AddSingleton<IInpatientService, InpatientService>();
        services.AddSingleton<IInvoiceService, InvoiceService>();
        services.AddSingleton<IPharmacyService, PharmacyService>();

        services.AddTransient<DashboardViewModel>();
        services.AddTransient<PatientsViewModel>();
        services.AddTransient<ClinicalDirectoryViewModel>();
        services.AddTransient<AppointmentsViewModel>();
        services.AddTransient<InpatientViewModel>();
        services.AddTransient<BillingViewModel>();
        services.AddTransient<PharmacyViewModel>();
        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();

        Services = services.BuildServiceProvider();
        MainWindow = Services.GetRequiredService<LoginWindow>();
        MainWindow.Show();
    }
}
