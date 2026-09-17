using CommunityToolkit.Mvvm.ComponentModel;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using System.Collections.ObjectModel;

namespace MediCoreHospital.UI.ViewModels.Pharmacy;

public partial class PharmacyViewModel : ObservableObject
{
    private readonly IPharmacyService _pharmacyService;

    [ObservableProperty] private string searchTerm = string.Empty;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;

    public ObservableCollection<PharmacyStockDto> LowStockItems { get; } = new();
    public ObservableCollection<PharmacyPrescriptionDto> Prescriptions { get; } = new();

    public PharmacyViewModel(IPharmacyService pharmacyService) => _pharmacyService = pharmacyService;

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var lowStockTask = _pharmacyService.GetLowStockAsync(cancellationToken);
            var prescriptionsTask = _pharmacyService.GetPrescriptionsAsync(SearchTerm, cancellationToken);

            await Task.WhenAll(lowStockTask, prescriptionsTask);

            LowStockItems.Clear();
            foreach (var item in lowStockTask.Result) LowStockItems.Add(item);

            Prescriptions.Clear();
            foreach (var item in prescriptionsTask.Result) Prescriptions.Add(item);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage = "تعذر تحميل بيانات الصيدلية. تحقق من اتصال قاعدة البيانات.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
