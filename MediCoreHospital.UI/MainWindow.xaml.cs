using System.Windows;
using MediCoreHospital.UI.Views.Dashboard;
using Microsoft.Extensions.DependencyInjection;

namespace MediCoreHospital.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var dashboardViewModel = ((App)Application.Current).Services.GetRequiredService<ViewModels.Dashboard.DashboardViewModel>();
        ContentHost.Content = new DashboardView(dashboardViewModel);
    }
}
