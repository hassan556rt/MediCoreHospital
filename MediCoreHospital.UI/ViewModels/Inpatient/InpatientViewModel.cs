using CommunityToolkit.Mvvm.ComponentModel;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using System.Collections.ObjectModel;

namespace MediCoreHospital.UI.ViewModels.Inpatient;

public partial class InpatientViewModel : ObservableObject
{
    private readonly IInpatientService _service;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;
    public ObservableCollection<InpatientAdmissionDto> Admissions { get; } = new();

    public InpatientViewModel(IInpatientService service) => _service = service;

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            var admissions = await _service.GetActiveAdmissionsAsync(cancellationToken);
            Admissions.Clear();
            foreach (var admission in admissions) Admissions.Add(admission);
        }
        catch (OperationCanceledException) { }
        catch (Exception) { ErrorMessage = "تعذر تحميل بيانات التنويم. تحقق من اتصال قاعدة البيانات."; }
        finally { IsLoading = false; }
    }
}
