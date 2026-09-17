using System.Data;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace MediCoreHospital.Infrastructure.Services;

public sealed class InpatientService : IInpatientService
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public InpatientService(ISqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<IReadOnlyList<InpatientAdmissionDto>> GetActiveAdmissionsAsync(CancellationToken cancellationToken = default)
    {
        var result = new List<InpatientAdmissionDto>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Admission_GetActive", connection) { CommandType = CommandType.StoredProcedure };
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new InpatientAdmissionDto
            {
                AdmissionId = reader.GetInt32(reader.GetOrdinal("AdmissionId")),
                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                PatientName = Read(reader, "PatientName"),
                DoctorName = Read(reader, "DoctorName"),
                RoomNumber = Read(reader, "RoomNumber"),
                BedNumber = Read(reader, "BedNumber"),
                AdmissionDate = reader.GetDateTime(reader.GetOrdinal("AdmissionDate")),
                DischargeDate = reader["DischargeDate"] == DBNull.Value ? null : reader.GetDateTime(reader.GetOrdinal("DischargeDate")),
                Diagnosis = Read(reader, "Diagnosis"),
                Status = Read(reader, "Status")
            });
        }
        return result;
    }

    private static string Read(SqlDataReader reader, string name) => reader[name] == DBNull.Value ? string.Empty : Convert.ToString(reader[name]) ?? string.Empty;
}
