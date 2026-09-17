using MediCoreHospital.Application.DTOs;

namespace MediCoreHospital.Application.Contracts;

public interface IPatientService
{
    Task<IReadOnlyList<PatientDto>> SearchAsync(string? searchTerm = null, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken = default);
}
