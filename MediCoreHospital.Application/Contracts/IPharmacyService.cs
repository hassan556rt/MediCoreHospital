using MediCoreHospital.Application.DTOs;

namespace MediCoreHospital.Application.Contracts;

public interface IPharmacyService
{
    Task<IReadOnlyList<PharmacyStockDto>> GetLowStockAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PharmacyPrescriptionDto>> GetPrescriptionsAsync(string? searchTerm = null, CancellationToken cancellationToken = default);
}
