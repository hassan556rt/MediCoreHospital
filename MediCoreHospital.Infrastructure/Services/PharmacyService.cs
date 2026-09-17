using System.Data;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace MediCoreHospital.Infrastructure.Services;

public sealed class PharmacyService : IPharmacyService
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public PharmacyService(ISqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<IReadOnlyList<PharmacyStockDto>> GetLowStockAsync(CancellationToken cancellationToken = default)
    {
        var result = new List<PharmacyStockDto>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Pharmacy_GetLowStock", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new PharmacyStockDto
            {
                StockId = reader.GetInt32(reader.GetOrdinal("StockId")),
                MedicineName = Read(reader, "MedicineName"),
                Category = Read(reader, "Category"),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                Unit = Read(reader, "Unit"),
                UnitPrice = reader["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["UnitPrice"]),
                ExpiryDate = reader.GetDateTime(reader.GetOrdinal("ExpiryDate"))
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<PharmacyPrescriptionDto>> GetPrescriptionsAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var result = new List<PharmacyPrescriptionDto>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Pharmacy_GetPrescriptions", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 150).Value = string.IsNullOrWhiteSpace(searchTerm) ? DBNull.Value : searchTerm.Trim();

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new PharmacyPrescriptionDto
            {
                PrescriptionId = reader.GetInt32(reader.GetOrdinal("PrescriptionId")),
                PatientName = Read(reader, "PatientName"),
                DoctorName = Read(reader, "DoctorName"),
                MedicineName = Read(reader, "MedicineName"),
                Dosage = Read(reader, "Dosage"),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                PrescriptionDate = reader.GetDateTime(reader.GetOrdinal("PrescriptionDate"))
            });
        }

        return result;
    }

    private static string Read(SqlDataReader reader, string name) => reader[name] == DBNull.Value ? string.Empty : Convert.ToString(reader[name]) ?? string.Empty;
}
