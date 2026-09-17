using System.Windows;
using System.Windows.Controls;
using MediCoreHospital.UI.ViewModels.Appointments;

namespace MediCoreHospital.UI.Views.Appointments;

public partial class AppointmentsView : UserControl
{
    private readonly AppointmentsViewModel _viewModel;
    public AppointmentsView(AppointmentsViewModel viewModel) { InitializeComponent(); _viewModel = viewModel; DataContext = viewModel; Loaded += OnLoaded; }
    private async void OnLoaded(object sender, RoutedEventArgs e) { Loaded -= OnLoaded; await _viewModel.LoadAsync(); }
    private async void Refresh_Click(object sender, RoutedEventArgs e) => await _viewModel.LoadAsync();
}
