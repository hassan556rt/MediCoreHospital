using System.Data;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace MediCoreHospital.Infrastructure.Services;

public sealed class PatientService : IPatientService
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public PatientService(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<PatientDto>> SearchAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var patients = new List<PatientDto>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Patient_Search", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 150).Value =
            string.IsNullOrWhiteSpace(searchTerm) ? DBNull.Value : searchTerm.Trim();

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            patients.Add(new PatientDto
            {
                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                MedicalRecordNumber = ReadString(reader, "MedicalRecordNumber"),
                NationalId = ReadString(reader, "NationalId"),
                FirstName = ReadString(reader, "FirstName"),
                LastName = ReadString(reader, "LastName"),
                Gender = ReadString(reader, "Gender"),
                DateOfBirth = ReadNullableDate(reader, "DateOfBirth"),
                Phone = ReadString(reader, "Phone"),
                BloodType = ReadString(reader, "BloodType"),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            });
        }

        return patients;
    }

    public async Task<int> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Patient_Create", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@MedicalRecordNumber", SqlDbType.NVarChar, 50).Value = request.MedicalRecordNumber.Trim();
        command.Parameters.Add("@NationalId", SqlDbType.NVarChar, 20).Value = DbValue(request.NationalId);
        command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100).Value = request.FirstName.Trim();
        command.Parameters.Add("@LastName", SqlDbType.NVarChar, 100).Value = request.LastName.Trim();
        command.Parameters.Add("@Gender", SqlDbType.NVarChar, 20).Value = DbValue(request.Gender);
        command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = request.DateOfBirth.HasValue ? request.DateOfBirth.Value.Date : DBNull.Value;
        command.Parameters.Add("@BloodType", SqlDbType.NVarChar, 10).Value = DbValue(request.BloodType);
        command.Parameters.Add("@Phone", SqlDbType.NVarChar, 30).Value = DbValue(request.Phone);
        command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = DbValue(request.Email);
        command.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = DbValue(request.Address);

        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    private static object DbValue(string? value) => string.IsNullOrWhiteSpace(value) ? DBNull.Value : value.Trim();
    private static string ReadString(SqlDataReader reader, string name) => reader[name] == DBNull.Value ? string.Empty : Convert.ToString(reader[name]) ?? string.Empty;
    private static DateTime? ReadNullableDate(SqlDataReader reader, string name) => reader[name] == DBNull.Value ? null : reader.GetDateTime(reader.GetOrdinal(name));
}
