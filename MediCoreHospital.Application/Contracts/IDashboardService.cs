using MediCoreHospital.Application.DTOs;

namespace MediCoreHospital.Application.Contracts;

public interface IDashboardService
{
    Task<DashboardStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UpcomingAppointment>> GetUpcomingAppointmentsAsync(int limit = 8, CancellationToken cancellationToken = default);
}
