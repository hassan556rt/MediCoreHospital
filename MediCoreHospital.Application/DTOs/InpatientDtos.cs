namespace MediCoreHospital.Application.DTOs;

public sealed class InpatientAdmissionDto
{
    public int AdmissionId { get; init; }
    public int PatientId { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public string DoctorName { get; init; } = string.Empty;
    public string RoomNumber { get; init; } = string.Empty;
    public string BedNumber { get; init; } = string.Empty;
    public DateTime AdmissionDate { get; init; }
    public DateTime? DischargeDate { get; init; }
    public string Diagnosis { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}
