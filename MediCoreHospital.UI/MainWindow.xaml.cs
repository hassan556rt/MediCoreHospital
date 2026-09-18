using System.Windows;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.UI.Views.Appointments;
using MediCoreHospital.UI.Views.Billing;
using MediCoreHospital.UI.Views.Clinical;
using MediCoreHospital.UI.Views.Dashboard;
using MediCoreHospital.UI.Views.Inpatient;
using MediCoreHospital.UI.Views.Patients;
using MediCoreHospital.UI.Views.Pharmacy;
using Microsoft.Extensions.DependencyInjection;

namespace MediCoreHospital.UI;

public partial class MainWindow : Window
{
    private readonly LoginResult _currentUser;
    public MainWindow(LoginResult currentUser)
    {
        InitializeComponent();
        _currentUser = currentUser;
        UserText.Text = $"{currentUser.FullName} · {currentUser.RoleName}";
        ShowDashboard();
    }

    private void Navigation_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.Tag is string page)
        {
            switch (page)
            {
                case "Patients": ShowPatients(); break;
                case "Clinical": ShowClinical(); break;
                case "Appointments": ShowAppointments(); break;
                case "Inpatient": ShowInpatient(); break;
                case "Billing": ShowBilling(); break;
                case "Pharmacy": ShowPharmacy(); break;
                default: ShowDashboard(); break;
            }
        }
    }

    private void ShowDashboard() => ContentHost.Content = new DashboardView(AppServices.GetRequiredService<DashboardViewModel>());
    private void ShowPatients() => ContentHost.Content = new PatientsView(AppServices.GetRequiredService<PatientsViewModel>());
    private void ShowClinical() => ContentHost.Content = new ClinicalDirectoryView(AppServices.GetRequiredService<ClinicalDirectoryViewModel>());
    private void ShowAppointments() => ContentHost.Content = new AppointmentsView(AppServices.GetRequiredService<AppointmentsViewModel>());
    private void ShowInpatient() => ContentHost.Content = new InpatientView(AppServices.GetRequiredService<InpatientViewModel>());
    private void ShowBilling() => ContentHost.Content = new BillingView(AppServices.GetRequiredService<BillingViewModel>());
    private void ShowPharmacy() => ContentHost.Content = new PharmacyView(AppServices.GetRequiredService<PharmacyViewModel>());

    private IServiceProvider AppServices => ((App)Application.Current).Services;

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        var login = AppServices.GetRequiredService<MediCoreHospital.UI.Views.Login.LoginWindow>();
        Application.Current.MainWindow = login;
        login.Show();
        Close();
    }
}
