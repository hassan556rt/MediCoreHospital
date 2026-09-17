using System.Data;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace MediCoreHospital.Infrastructure.Services;

public sealed class AppointmentService : IAppointmentService
{
    private readonly ISqlConnectionFactory _connectionFactory;
    public AppointmentService(ISqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<IReadOnlyList<AppointmentDto>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var result = new List<AppointmentDto>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Appointment_GetByDate", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@AppointmentDate", SqlDbType.Date).Value = date.Date;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new AppointmentDto
            {
                AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                PatientName = Read(reader, "PatientName"),
                DoctorName = Read(reader, "DoctorName"),
                DepartmentName = Read(reader, "DepartmentName"),
                AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                EndTime = reader.GetTimeSpan(reader.GetOrdinal("EndTime")),
                Status = Read(reader, "Status"),
                Reason = Read(reader, "Reason")
            });
        }
        return result;
    }

    public async Task<int> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Appointment_Create", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PatientId", SqlDbType.Int).Value = request.PatientId;
        command.Parameters.Add("@DoctorId", SqlDbType.Int).Value = request.DoctorId;
        command.Parameters.Add("@AppointmentDate", SqlDbType.Date).Value = request.AppointmentDate.Date;
        command.Parameters.Add("@StartTime", SqlDbType.Time).Value = request.StartTime;
        command.Parameters.Add("@EndTime", SqlDbType.Time).Value = request.EndTime;
        command.Parameters.Add("@Reason", SqlDbType.NVarChar, 250).Value = string.IsNullOrWhiteSpace(request.Reason) ? DBNull.Value : request.Reason.Trim();
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    public async Task CancelAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Appointment_Cancel", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@AppointmentId", SqlDbType.Int).Value = appointmentId;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static string Read(SqlDataReader reader, string name) => reader[name] == DBNull.Value ? string.Empty : Convert.ToString(reader[name]) ?? string.Empty;
}
