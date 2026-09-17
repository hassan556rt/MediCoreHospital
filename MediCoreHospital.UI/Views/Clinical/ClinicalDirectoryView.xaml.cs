using System.Windows;
using System.Windows.Controls;
using MediCoreHospital.UI.ViewModels.Clinical;

namespace MediCoreHospital.UI.Views.Clinical;

public partial class ClinicalDirectoryView : UserControl
{
    private readonly ClinicalDirectoryViewModel _viewModel;
    public ClinicalDirectoryView(ClinicalDirectoryViewModel viewModel) { InitializeComponent(); _viewModel = viewModel; DataContext = viewModel; Loaded += LoadedAsync; }
    private async void LoadedAsync(object sender, RoutedEventArgs e) { Loaded -= LoadedAsync; await _viewModel.LoadAsync(); }
    private async void Refresh_Click(object sender, RoutedEventArgs e) => await _viewModel.LoadAsync();
}
