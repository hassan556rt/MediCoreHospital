using MediCoreHospital.Application.DTOs;

namespace MediCoreHospital.Application.Contracts;

public interface IPatientService
{
    Task<IReadOnlyList<PatientDto>> SearchAsync(string? searchTerm = null, CancellationToken cancellationToken = default);
    Task<PatientDto?> GetByIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdatePatientRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int patientId, CancellationToken cancellationToken = default);
}
