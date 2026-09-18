using System.Windows;
using MediCoreHospital.UI.ViewModels.Patients;

namespace MediCoreHospital.UI.Views.Patients;

public partial class PatientsView : System.Windows.Controls.UserControl
{
    private readonly PatientsViewModel _viewModel;
    public PatientsView(PatientsViewModel viewModel) { InitializeComponent(); _viewModel = viewModel; DataContext = viewModel; Loaded += OnLoaded; }
    private async void OnLoaded(object sender, RoutedEventArgs e) { Loaded -= OnLoaded; await _viewModel.LoadAsync(); }
    private async void Search_Click(object sender, RoutedEventArgs e) => await _viewModel.LoadAsync();
    private async void Refresh_Click(object sender, RoutedEventArgs e) => await _viewModel.LoadAsync();
    private async void Save_Click(object sender, RoutedEventArgs e) => await _viewModel.SaveAsync();
    private async void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("هل أنت متأكد من حذف المريض المحدد؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            await _viewModel.DeleteAsync();
    }
    private void New_Click(object sender, RoutedEventArgs e) => _viewModel.NewPatient();
}
