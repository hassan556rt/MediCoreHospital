using CommunityToolkit.Mvvm.ComponentModel;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;

namespace MediCoreHospital.UI.ViewModels.Patients;

public partial class PatientsViewModel : ObservableObject
{
    private readonly IPatientService _patientService;

    [ObservableProperty] private string searchTerm = string.Empty;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;
    [ObservableProperty] private string successMessage = string.Empty;
    [ObservableProperty] private string firstName = string.Empty;
    [ObservableProperty] private string lastName = string.Empty;
    [ObservableProperty] private string medicalRecordNumber = string.Empty;
    [ObservableProperty] private string phone = string.Empty;
    [ObservableProperty] private string nationalId = string.Empty;
    [ObservableProperty] private string gender = string.Empty;

    public ObservableCollection<PatientDto> Patients { get; } = new();
    public PatientsViewModel(IPatientService patientService) => _patientService = patientService;

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        await ExecuteAsync(async () =>
        {
            var result = await _patientService.SearchAsync(SearchTerm, cancellationToken);
            Patients.Clear();
            foreach (var patient in result) Patients.Add(patient);
        });
    }

    public async Task CreateAsync(CancellationToken cancellationToken = default)
    {
        ErrorMessage = SuccessMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(FirstName)) { ErrorMessage = "يرجى إدخال الاسم الأول للمريض."; return; }
        if (string.IsNullOrWhiteSpace(LastName)) { ErrorMessage = "يرجى إدخال اسم العائلة للمريض."; return; }
        if (string.IsNullOrWhiteSpace(MedicalRecordNumber)) { ErrorMessage = "يرجى إدخال رقم الملف الطبي."; return; }

        await ExecuteAsync(async () =>
        {
            await _patientService.CreateAsync(new CreatePatientRequest
            {
                FirstName = FirstName, LastName = LastName, MedicalRecordNumber = MedicalRecordNumber,
                Phone = Phone, NationalId = NationalId, Gender = Gender
            }, cancellationToken);
            SuccessMessage = "تم حفظ بيانات المريض بنجاح.";
            FirstName = LastName = MedicalRecordNumber = Phone = NationalId = Gender = string.Empty;
            await LoadAsync(cancellationToken);
        });
    }

    private async Task ExecuteAsync(Func<Task> operation)
    {
        try { IsLoading = true; ErrorMessage = string.Empty; await operation(); }
        catch (OperationCanceledException) { }
        catch (SqlException) { ErrorMessage = "تعذر تنفيذ العملية. يرجى التحقق من قاعدة البيانات والبيانات المدخلة."; }
        catch (Exception) { ErrorMessage = "حدث خطأ غير متوقع أثناء تنفيذ العملية."; }
        finally { IsLoading = false; }
    }
}
