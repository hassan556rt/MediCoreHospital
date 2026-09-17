namespace MediCoreHospital.Application.DTOs;

public sealed class DepartmentDto
{
    public int DepartmentId { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}

public sealed class DoctorDto
{
    public int DoctorId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Specialty { get; init; } = string.Empty;
    public string LicenseNumber { get; init; } = string.Empty;
    public string DepartmentName { get; init; } = string.Empty;
    public decimal ConsultationFee { get; init; }
    public bool IsActive { get; init; }
}
