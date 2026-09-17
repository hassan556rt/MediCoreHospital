using MediCoreHospital.Application.DTOs;

namespace MediCoreHospital.Application.Contracts;

public interface IInvoiceService
{
    Task<IReadOnlyList<InvoiceDto>> GetOpenInvoicesAsync(CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default);
}
