using System.Data;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace MediCoreHospital.Infrastructure.Services;

public sealed class InvoiceService : IInvoiceService
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public InvoiceService(ISqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<IReadOnlyList<InvoiceDto>> GetOpenInvoicesAsync(CancellationToken cancellationToken = default)
    {
        var invoices = new List<InvoiceDto>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Invoice_GetOpen", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            invoices.Add(new InvoiceDto
            {
                InvoiceId = reader.GetInt32(reader.GetOrdinal("InvoiceId")),
                PatientName = Read(reader, "PatientName"),
                InvoiceNumber = Read(reader, "InvoiceNumber"),
                IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                TotalAmount = reader["TotalAmount"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["TotalAmount"]),
                PaidAmount = reader["PaidAmount"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["PaidAmount"]),
                Status = Read(reader, "Status")
            });
        }

        return invoices;
    }

    public async Task<int> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Invoice_Create", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@PatientId", SqlDbType.Int).Value = request.PatientId;
        command.Parameters.Add("@InvoiceNumber", SqlDbType.NVarChar, 50).Value = request.InvoiceNumber.Trim();
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal, 18).Value = request.TotalAmount;
        command.Parameters.Add("@PaidAmount", SqlDbType.Decimal, 18).Value = request.PaidAmount;
        command.Parameters.Add("@Status", SqlDbType.NVarChar, 30).Value = string.IsNullOrWhiteSpace(request.Status) ? "Open" : request.Status.Trim();

        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    private static string Read(SqlDataReader reader, string name) => reader[name] == DBNull.Value ? string.Empty : Convert.ToString(reader[name]) ?? string.Empty;
}
