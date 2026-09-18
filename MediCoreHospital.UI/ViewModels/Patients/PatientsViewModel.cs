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
    [ObservableProperty] private PatientDto? selectedPatient;
    [ObservableProperty] private string firstName = string.Empty;
    [ObservableProperty] private string lastName = string.Empty;
    [ObservableProperty] private string medicalRecordNumber = string.Empty;
    [ObservableProperty] private string phone = string.Empty;
    [ObservableProperty] private string nationalId = string.Empty;
    [ObservableProperty] private string gender = string.Empty;
    [ObservableProperty] private DateTime? dateOfBirth;
    [ObservableProperty] private string bloodType = string.Empty;
    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string address = string.Empty;
    public ObservableCollection<PatientDto> Patients { get; } = new();
    public bool IsEditing => SelectedPatient is not null;
    public PatientsViewModel(IPatientService patientService) => _patientService = patientService;

    partial void OnSelectedPatientChanged(PatientDto? value)
    {
        if (value is null) { ClearForm(); }
        else { FirstName = value.FirstName; LastName = value.LastName; MedicalRecordNumber = value.MedicalRecordNumber; NationalId = value.NationalId; Gender = value.Gender; DateOfBirth = value.DateOfBirth; BloodType = value.BloodType; Phone = value.Phone; Email = value.Email; Address = value.Address; }
        OnPropertyChanged(nameof(IsEditing));
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default) => await ExecuteAsync(async () =>
    {
        var result = await _patientService.SearchAsync(SearchTerm, cancellationToken);
        Patients.Clear(); foreach (var patient in result) Patients.Add(patient);
    });

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        ErrorMessage = SuccessMessage = string.Empty;
        if (!Validate()) return;
        await ExecuteAsync(async () =>
        {
            if (SelectedPatient is null)
            {
                await _patientService.CreateAsync(ToCreateRequest(), cancellationToken);
                SuccessMessage = "تمت إضافة المريض بنجاح.";
            }
            else
            {
                await _patientService.UpdateAsync(new UpdatePatientRequest { PatientId = SelectedPatient.PatientId, MedicalRecordNumber = MedicalRecordNumber, NationalId = NationalId, FirstName = FirstName, LastName = LastName, Gender = Gender, DateOfBirth = DateOfBirth, BloodType = BloodType, Phone = Phone, Email = Email, Address = Address }, cancellationToken);
                SuccessMessage = "تم تحديث بيانات المريض بنجاح.";
            }
            ClearForm(); await LoadAsync(cancellationToken);
        });
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        if (SelectedPatient is null) { ErrorMessage = "يرجى اختيار مريض للحذف."; return; }
        await ExecuteAsync(async () => { await _patientService.DeleteAsync(SelectedPatient.PatientId, cancellationToken); SelectedPatient = null; SuccessMessage = "تم حذف المريض بنجاح."; await LoadAsync(cancellationToken); });
    }

    public void NewPatient() { SelectedPatient = null; ClearForm(); ErrorMessage = SuccessMessage = string.Empty; }
    private CreatePatientRequest ToCreateRequest() => new() { MedicalRecordNumber = MedicalRecordNumber, NationalId = NationalId, FirstName = FirstName, LastName = LastName, Gender = Gender, DateOfBirth = DateOfBirth, BloodType = BloodType, Phone = Phone, Email = Email, Address = Address };
    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName)) { ErrorMessage = "يرجى إدخال الاسم الأول للمريض."; return false; }
        if (string.IsNullOrWhiteSpace(LastName)) { ErrorMessage = "يرجى إدخال اسم العائلة للمريض."; return false; }
        if (string.IsNullOrWhiteSpace(MedicalRecordNumber)) { ErrorMessage = "يرجى إدخال رقم الملف الطبي."; return false; }
        if (DateOfBirth > DateTime.Today) { ErrorMessage = "تاريخ الميلاد لا يمكن أن يكون في المستقبل."; return false; }
        return true;
    }
    private void ClearForm() { FirstName = LastName = MedicalRecordNumber = Phone = NationalId = Gender = BloodType = Email = Address = string.Empty; DateOfBirth = null; }
    private async Task ExecuteAsync(Func<Task> operation)
    {
        try { IsLoading = true; ErrorMessage = string.Empty; await operation(); }
        catch (OperationCanceledException) { }
        catch (SqlException) { ErrorMessage = "تعذر تنفيذ العملية. يرجى التحقق من قاعدة البيانات أو رقم الملف الطبي."; }
        catch (Exception) { ErrorMessage = "حدث خطأ غير متوقع أثناء تنفيذ العملية."; }
        finally { IsLoading = false; }
    }
}
