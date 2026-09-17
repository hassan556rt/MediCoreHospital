namespace MediCoreHospital.Application.DTOs;

public sealed class InvoiceDto
{
    public int InvoiceId { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public string InvoiceNumber { get; init; } = string.Empty;
    public DateTime IssueDate { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal PaidAmount { get; init; }
    public decimal Balance => TotalAmount - PaidAmount;
    public string Status { get; init; } = string.Empty;
}

public sealed class CreateInvoiceRequest
{
    public int PatientId { get; init; }
    public string InvoiceNumber { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public decimal PaidAmount { get; init; }
    public string Status { get; init; } = "Open";
}
