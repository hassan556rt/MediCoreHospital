using CommunityToolkit.Mvvm.ComponentModel;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using System.Collections.ObjectModel;

namespace MediCoreHospital.UI.ViewModels.Clinical;

public partial class ClinicalDirectoryViewModel : ObservableObject
{
    private readonly IClinicalDirectoryService _service;
    [ObservableProperty] private string searchTerm = string.Empty;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;
    public ObservableCollection<DoctorDto> Doctors { get; } = new();
    public ObservableCollection<DepartmentDto> Departments { get; } = new();

    public ClinicalDirectoryViewModel(IClinicalDirectoryService service) => _service = service;

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            var doctorsTask = _service.GetDoctorsAsync(SearchTerm, cancellationToken);
            var departmentsTask = _service.GetDepartmentsAsync(cancellationToken);
            await Task.WhenAll(doctorsTask, departmentsTask);
            Doctors.Clear();
            foreach (var item in doctorsTask.Result) Doctors.Add(item);
            Departments.Clear();
            foreach (var item in departmentsTask.Result) Departments.Add(item);
        }
        catch (OperationCanceledException) { }
        catch (Exception) { ErrorMessage = "تعذر تحميل بيانات الأطباء والأقسام. تحقق من اتصال قاعدة البيانات."; }
        finally { IsLoading = false; }
    }
}
