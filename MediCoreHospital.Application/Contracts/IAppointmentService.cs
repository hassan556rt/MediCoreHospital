using MediCoreHospital.Application.DTOs;

namespace MediCoreHospital.Application.Contracts;

public interface IAppointmentService
{
    Task<IReadOnlyList<AppointmentDto>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default);
    Task CancelAsync(int appointmentId, CancellationToken cancellationToken = default);
}
