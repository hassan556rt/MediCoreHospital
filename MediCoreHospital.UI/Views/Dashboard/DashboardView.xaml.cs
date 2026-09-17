using System.Windows.Controls;
using MediCoreHospital.UI.ViewModels.Dashboard;

namespace MediCoreHospital.UI.Views.Dashboard;

public partial class DashboardView : UserControl
{
    private readonly DashboardViewModel _viewModel;

    public DashboardView(DashboardViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        Loaded += DashboardView_Loaded;
    }

    private async void DashboardView_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        Loaded -= DashboardView_Loaded;
        await _viewModel.LoadAsync();
    }
}
