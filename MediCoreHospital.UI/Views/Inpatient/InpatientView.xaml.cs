using System.Windows;
using System.Windows.Controls;
using MediCoreHospital.UI.ViewModels.Inpatient;

namespace MediCoreHospital.UI.Views.Inpatient;

public partial class InpatientView : UserControl
{
    private readonly InpatientViewModel _viewModel;
    public InpatientView(InpatientViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        Loaded += OnLoaded;
    }
    private async void OnLoaded(object sender, RoutedEventArgs e) { Loaded -= OnLoaded; await _viewModel.LoadAsync(); }
    private async void Refresh_Click(object sender, RoutedEventArgs e) => await _viewModel.LoadAsync();
}
