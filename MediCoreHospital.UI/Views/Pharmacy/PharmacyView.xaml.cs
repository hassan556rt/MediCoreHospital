using System.Windows;
using System.Windows.Controls;
using MediCoreHospital.UI.ViewModels.Pharmacy;

namespace MediCoreHospital.UI.Views.Pharmacy;

public partial class PharmacyView : UserControl
{
    private readonly PharmacyViewModel _viewModel;

    public PharmacyView(PharmacyViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        await _viewModel.LoadAsync();
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e) => await _viewModel.LoadAsync();
}
