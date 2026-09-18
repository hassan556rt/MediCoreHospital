namespace MediCoreHospital.Application.DTOs;

public sealed class PatientDto
{
    public int PatientId { get; init; }
    public string MedicalRecordNumber { get; init; } = string.Empty;
    public string NationalId { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string Gender { get; init; } = string.Empty;
    public DateTime? DateOfBirth { get; init; }
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string BloodType { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

public sealed class CreatePatientRequest
{
    public string MedicalRecordNumber { get; init; } = string.Empty;
    public string NationalId { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Gender { get; init; } = string.Empty;
    public DateTime? DateOfBirth { get; init; }
    public string BloodType { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
}

public sealed class UpdatePatientRequest : CreatePatientRequest
{
    public int PatientId { get; init; }
}
