using MediCoreHospital.Application.DTOs;

namespace MediCoreHospital.Application.Contracts;

public interface IClinicalDirectoryService
{
    Task<IReadOnlyList<DepartmentDto>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DoctorDto>> GetDoctorsAsync(string? searchTerm = null, CancellationToken cancellationToken = default);
}
