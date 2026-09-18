using System.Windows;
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
    public MainWindow()
    {
        InitializeComponent();
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

    private void ShowDashboard() => ContentHost.Content = new DashboardView(((App)global::System.Windows.Application.Current).Services.GetRequiredService<ViewModels.Dashboard.DashboardViewModel>());
    private void ShowPatients() => ContentHost.Content = new PatientsView(((App)global::System.Windows.Application.Current).Services.GetRequiredService<ViewModels.Patients.PatientsViewModel>());
    private void ShowClinical() => ContentHost.Content = new ClinicalDirectoryView(((App)global::System.Windows.Application.Current).Services.GetRequiredService<ViewModels.Clinical.ClinicalDirectoryViewModel>());
    private void ShowAppointments() => ContentHost.Content = new AppointmentsView(((App)global::System.Windows.Application.Current).Services.GetRequiredService<ViewModels.Appointments.AppointmentsViewModel>());
    private void ShowInpatient() => ContentHost.Content = new InpatientView(((App)global::System.Windows.Application.Current).Services.GetRequiredService<ViewModels.Inpatient.InpatientViewModel>());
    private void ShowBilling() => ContentHost.Content = new BillingView(((App)global::System.Windows.Application.Current).Services.GetRequiredService<ViewModels.Billing.BillingViewModel>());
    private void ShowPharmacy() => ContentHost.Content = new PharmacyView(((App)global::System.Windows.Application.Current).Services.GetRequiredService<ViewModels.Pharmacy.PharmacyViewModel>());
    private void Logout_Click(object sender, RoutedEventArgs e) => Close();
}
