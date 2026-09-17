namespace MediCoreHospital.Application.DTOs;

public sealed class DashboardStatistics
{
    public int TotalPatients { get; init; }
    public int Doctors { get; init; }
    public int TodayAppointments { get; init; }
    public int Inpatients { get; init; }
    public int AvailableBeds { get; init; }
    public int OccupiedBeds { get; init; }
    public decimal DailyRevenue { get; init; }
    public decimal OutstandingInvoices { get; init; }
}

public sealed class UpcomingAppointment
{
    public int AppointmentId { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public string DoctorName { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public DateTime AppointmentDate { get; init; }
    public TimeSpan StartTime { get; init; }
    public string Status { get; init; } = string.Empty;
    public string DisplayTime => StartTime.ToString(@"hh\:mm");
}
