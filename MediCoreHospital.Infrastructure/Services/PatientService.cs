using System.Data;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace MediCoreHospital.Infrastructure.Services;

public sealed class PatientService : IPatientService
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public PatientService(ISqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<IReadOnlyList<PatientDto>> SearchAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var patients = new List<PatientDto>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand("Patient_Search", connection);
        command.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 150).Value = DbValue(searchTerm);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            patients.Add(Map(reader));
        return patients;
    }

    public async Task<PatientDto?> GetByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand("Patient_GetById", connection);
        command.Parameters.Add("@PatientId", SqlDbType.Int).Value = patientId;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? Map(reader) : null;
    }

    public async Task<int> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand("Patient_Create", connection);
        Add(command, "@MedicalRecordNumber", SqlDbType.NVarChar, 50, request.MedicalRecordNumber, true);
        Add(command, "@NationalId", SqlDbType.NVarChar, 20, request.NationalId);
        Add(command, "@FirstName", SqlDbType.NVarChar, 100, request.FirstName, true);
        Add(command, "@LastName", SqlDbType.NVarChar, 100, request.LastName, true);
        Add(command, "@Gender", SqlDbType.NVarChar, 20, request.Gender);
        AddDate(command, request.DateOfBirth);
        Add(command, "@BloodType", SqlDbType.NVarChar, 10, request.BloodType);
        Add(command, "@Phone", SqlDbType.NVarChar, 30, request.Phone);
        Add(command, "@Email", SqlDbType.NVarChar, 150, request.Email);
        Add(command, "@Address", SqlDbType.NVarChar, 250, request.Address);
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    public async Task UpdateAsync(UpdatePatientRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand("Patient_Update", connection);
        command.Parameters.Add("@PatientId", SqlDbType.Int).Value = request.PatientId;
        Add(command, "@MedicalRecordNumber", SqlDbType.NVarChar, 50, request.MedicalRecordNumber, true);
        Add(command, "@NationalId", SqlDbType.NVarChar, 20, request.NationalId);
        Add(command, "@FirstName", SqlDbType.NVarChar, 100, request.FirstName, true);
        Add(command, "@LastName", SqlDbType.NVarChar, 100, request.LastName, true);
        Add(command, "@Gender", SqlDbType.NVarChar, 20, request.Gender);
        AddDate(command, request.DateOfBirth);
        Add(command, "@BloodType", SqlDbType.NVarChar, 10, request.BloodType);
        Add(command, "@Phone", SqlDbType.NVarChar, 30, request.Phone);
        Add(command, "@Email", SqlDbType.NVarChar, 150, request.Email);
        Add(command, "@Address", SqlDbType.NVarChar, 250, request.Address);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteAsync(int patientId, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand("Patient_Delete", connection);
        command.Parameters.Add("@PatientId", SqlDbType.Int).Value = patientId;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static SqlCommand CreateCommand(string name, SqlConnection connection) => new(name, connection) { CommandType = CommandType.StoredProcedure };
    private static void AddDate(SqlCommand command, DateTime? value) => command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = value?.Date ?? (object)DBNull.Value;
    private static void Add(SqlCommand command, string name, SqlDbType type, int size, string? value, bool required = false) => command.Parameters.Add(name, type, size).Value = required ? value!.Trim() : DbValue(value);
    private static object DbValue(string? value) => string.IsNullOrWhiteSpace(value) ? DBNull.Value : value.Trim();

    private static PatientDto Map(SqlDataReader reader) => new()
    {
        PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
        MedicalRecordNumber = ReadString(reader, "MedicalRecordNumber"),
        NationalId = ReadString(reader, "NationalId"),
        FirstName = ReadString(reader, "FirstName"),
        LastName = ReadString(reader, "LastName"),
        Gender = ReadString(reader, "Gender"),
        DateOfBirth = ReadNullableDate(reader, "DateOfBirth"),
        BloodType = ReadString(reader, "BloodType"),
        Phone = ReadString(reader, "Phone"),
        Email = ReadString(reader, "Email"),
        Address = ReadString(reader, "Address"),
        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
    };

    private static string ReadString(SqlDataReader reader, string name) => reader[name] == DBNull.Value ? string.Empty : Convert.ToString(reader[name]) ?? string.Empty;
    private static DateTime? ReadNullableDate(SqlDataReader reader, string name) => reader[name] == DBNull.Value ? null : reader.GetDateTime(reader.GetOrdinal(name));
}
