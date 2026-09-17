using System.Windows;
using MediCoreHospital.UI.Views.Appointments;
using MediCoreHospital.UI.Views.Clinical;
using MediCoreHospital.UI.Views.Dashboard;
using MediCoreHospital.UI.Views.Patients;
using Microsoft.Extensions.DependencyInjection;

namespace MediCoreHospital.UI;

public partial class MainWindow : Window
{
    public MainWindow() { InitializeComponent(); ShowDashboard(); }
    private void Navigation_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.Tag is string page)
        {
            switch (page) { case "Patients": ShowPatients(); break; case "Clinical": ShowClinical(); break; case "Appointments": ShowAppointments(); break; default: ShowDashboard(); break; }
        }
    }
    private void ShowDashboard() => ContentHost.Content = new DashboardView(((App)Application.Current).Services.GetRequiredService<ViewModels.Dashboard.DashboardViewModel>());
    private void ShowPatients() => ContentHost.Content = new PatientsView(((App)Application.Current).Services.GetRequiredService<ViewModels.Patients.PatientsViewModel>());
    private void ShowClinical() => ContentHost.Content = new ClinicalDirectoryView(((App)Application.Current).Services.GetRequiredService<ViewModels.Clinical.ClinicalDirectoryViewModel>());
    private void ShowAppointments() => ContentHost.Content = new AppointmentsView(((App)Application.Current).Services.GetRequiredService<ViewModels.Appointments.AppointmentsViewModel>());
    private void Logout_Click(object sender, RoutedEventArgs e) => Close();
}
