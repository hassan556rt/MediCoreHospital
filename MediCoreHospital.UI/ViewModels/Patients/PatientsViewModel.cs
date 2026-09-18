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
        if (value is null) ClearForm();
        else _ = LoadSelectedPatientAsync(value.PatientId);
        OnPropertyChanged(nameof(IsEditing));
    }

    private async Task LoadSelectedPatientAsync(int patientId)
    {
        try
        {
            var patient = await _patientService.GetByIdAsync(patientId);
            if (patient is not null && SelectedPatient?.PatientId == patientId) PopulateForm(patient);
        }
        catch (SqlException) { ErrorMessage = "تعذر تحميل تفاصيل المريض من قاعدة البيانات."; }
        catch (Exception) { ErrorMessage = "حدث خطأ غير متوقع أثناء تحميل تفاصيل المريض."; }
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        await ExecuteAsync(async () =>
        {
            var result = await _patientService.SearchAsync(SearchTerm, cancellationToken);
            Patients.Clear();
            foreach (var patient in result) Patients.Add(patient);
        }, cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        ErrorMessage = SuccessMessage = string.Empty;
        if (IsLoading || !Validate()) return;

        await ExecuteAsync(async () =>
        {
            if (SelectedPatient is null)
            {
                await _patientService.CreateAsync(ToCreateRequest(), cancellationToken);
                SuccessMessage = "تمت إضافة المريض بنجاح.";
            }
            else
            {
                await _patientService.UpdateAsync(new UpdatePatientRequest
                {
                    PatientId = SelectedPatient.PatientId, MedicalRecordNumber = MedicalRecordNumber,
                    NationalId = NationalId, FirstName = FirstName, LastName = LastName, Gender = Gender,
                    DateOfBirth = DateOfBirth, BloodType = BloodType, Phone = Phone, Email = Email, Address = Address
                }, cancellationToken);
                SuccessMessage = "تم تحديث بيانات المريض بنجاح.";
            }

            await ReloadPatientsAsync(cancellationToken);
            ClearForm();
        }, cancellationToken);
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        if (IsLoading) return;
        if (SelectedPatient is null) { ErrorMessage = "يرجى اختيار مريض للحذف."; return; }

        var patientId = SelectedPatient.PatientId;
        await ExecuteAsync(async () =>
        {
            await _patientService.DeleteAsync(patientId, cancellationToken);
            await ReloadPatientsAsync(cancellationToken);
            ClearForm();
            SuccessMessage = "تم حذف المريض بنجاح.";
        }, cancellationToken);
    }

    public void NewPatient() => ClearForm();
    public void CancelEdit() => ClearForm();

    private async Task ReloadPatientsAsync(CancellationToken cancellationToken)
    {
        var result = await _patientService.SearchAsync(SearchTerm, cancellationToken);
        Patients.Clear();
        foreach (var patient in result) Patients.Add(patient);
    }

    private void PopulateForm(PatientDto patient)
    {
        FirstName = patient.FirstName; LastName = patient.LastName; MedicalRecordNumber = patient.MedicalRecordNumber;
        NationalId = patient.NationalId; Gender = patient.Gender; DateOfBirth = patient.DateOfBirth;
        BloodType = patient.BloodType; Phone = patient.Phone; Email = patient.Email; Address = patient.Address;
    }

    private CreatePatientRequest ToCreateRequest() => new()
    {
        MedicalRecordNumber = MedicalRecordNumber, NationalId = NationalId, FirstName = FirstName,
        LastName = LastName, Gender = Gender, DateOfBirth = DateOfBirth, BloodType = BloodType,
        Phone = Phone, Email = Email, Address = Address
    };

    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName)) return SetError("يرجى إدخال الاسم الأول للمريض.");
        if (string.IsNullOrWhiteSpace(LastName)) return SetError("يرجى إدخال اسم العائلة للمريض.");
        if (string.IsNullOrWhiteSpace(MedicalRecordNumber)) return SetError("يرجى إدخال رقم الملف الطبي.");
        if (MedicalRecordNumber.Trim().Length > 50) return SetError("رقم الملف الطبي يجب ألا يتجاوز 50 حرفاً.");
        if (DateOfBirth > DateTime.Today) return SetError("تاريخ الميلاد لا يمكن أن يكون في المستقبل.");
        return true;
    }

    private bool SetError(string message) { ErrorMessage = message; return false; }
    private void ClearForm() { SelectedPatient = null; FirstName = LastName = MedicalRecordNumber = Phone = NationalId = Gender = BloodType = Email = Address = string.Empty; DateOfBirth = null; OnPropertyChanged(nameof(IsEditing)); }

    private async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken)
    {
        try { IsLoading = true; ErrorMessage = string.Empty; await operation(); }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { }
        catch (SqlException) { ErrorMessage = "تعذر تنفيذ العملية. يرجى التحقق من قاعدة البيانات أو رقم الملف الطبي."; }
        catch (Exception) { ErrorMessage = "حدث خطأ غير متوقع أثناء تنفيذ العملية."; }
        finally { IsLoading = false; }
    }
}
