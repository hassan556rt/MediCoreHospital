using System.Windows;
using System.Windows.Controls;
using MediCoreHospital.UI.ViewModels.Billing;

namespace MediCoreHospital.UI.Views.Billing;

public partial class BillingView : UserControl
{
    private readonly BillingViewModel _viewModel;

    public BillingView(BillingViewModel viewModel)
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
