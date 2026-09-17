using System.Data;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace MediCoreHospital.Infrastructure.Services;

public sealed class ClinicalDirectoryService : IClinicalDirectoryService
{
    private readonly ISqlConnectionFactory _connectionFactory;
    public ClinicalDirectoryService(ISqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<IReadOnlyList<DepartmentDto>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        var result = new List<DepartmentDto>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Department_GetActive", connection) { CommandType = CommandType.StoredProcedure };
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new DepartmentDto
            {
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                DepartmentName = Read(reader, "DepartmentName"),
                Description = Read(reader, "Description")
            });
        }
        return result;
    }

    public async Task<IReadOnlyList<DoctorDto>> GetDoctorsAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var result = new List<DoctorDto>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Doctor_Search", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 150).Value = string.IsNullOrWhiteSpace(searchTerm) ? DBNull.Value : searchTerm.Trim();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new DoctorDto
            {
                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                FullName = Read(reader, "FullName"),
                Specialty = Read(reader, "Specialty"),
                LicenseNumber = Read(reader, "LicenseNumber"),
                DepartmentName = Read(reader, "DepartmentName"),
                ConsultationFee = reader["ConsultationFee"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ConsultationFee"]),
                IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"])
            });
        }
        return result;
    }

    private static string Read(SqlDataReader reader, string name) => reader[name] == DBNull.Value ? string.Empty : Convert.ToString(reader[name]) ?? string.Empty;
}
