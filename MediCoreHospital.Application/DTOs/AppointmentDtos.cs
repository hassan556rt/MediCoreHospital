namespace MediCoreHospital.Application.DTOs;

public sealed class AppointmentDto
{
    public int AppointmentId { get; init; }
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public string DoctorName { get; init; } = string.Empty;
    public string DepartmentName { get; init; } = string.Empty;
    public DateTime AppointmentDate { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
}

public sealed class CreateAppointmentRequest
{
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public DateTime AppointmentDate { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public string Reason { get; init; } = string.Empty;
}
