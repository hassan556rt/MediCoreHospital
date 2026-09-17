using System.Windows;
using System.Windows.Controls;
using MediCoreHospital.UI.ViewModels.Patients;

namespace MediCoreHospital.UI.Views.Patients;

public partial class PatientsView : UserControl
{
    private readonly PatientsViewModel _viewModel;
    public PatientsView(PatientsViewModel viewModel) { InitializeComponent(); _viewModel = viewModel; DataContext = viewModel; Loaded += OnLoaded; }
    private async void OnLoaded(object sender, RoutedEventArgs e) { Loaded -= OnLoaded; await _viewModel.LoadAsync(); }
    private async void Search_Click(object sender, RoutedEventArgs e) => await _viewModel.LoadAsync();
    private async void Refresh_Click(object sender, RoutedEventArgs e) => await _viewModel.LoadAsync();
    private async void Create_Click(object sender, RoutedEventArgs e) => await _viewModel.CreateAsync();
}
