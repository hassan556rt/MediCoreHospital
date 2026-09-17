using MediCoreHospital.Application.DTOs;

namespace MediCoreHospital.Application.Contracts;

public interface IInpatientService
{
    Task<IReadOnlyList<InpatientAdmissionDto>> GetActiveAdmissionsAsync(CancellationToken cancellationToken = default);
}
