namespace MediCoreHospital.Application.DTOs;

public sealed class PharmacyStockDto
{
    public int StockId { get; init; }
    public string MedicineName { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public string Unit { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public DateTime ExpiryDate { get; init; }
}

public sealed class PharmacyPrescriptionDto
{
    public int PrescriptionId { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public string DoctorName { get; init; } = string.Empty;
    public string MedicineName { get; init; } = string.Empty;
    public string Dosage { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public DateTime PrescriptionDate { get; init; }
}
