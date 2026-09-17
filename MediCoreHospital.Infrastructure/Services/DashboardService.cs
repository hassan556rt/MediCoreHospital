using System.Data;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace MediCoreHospital.Infrastructure.Services;

public sealed class DashboardService : IDashboardService
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DashboardService(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<DashboardStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Dashboard_GetStatistics", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return new DashboardStatistics();
        }

        return new DashboardStatistics
        {
            TotalPatients = ReadInt(reader, "TotalPatients"),
            Doctors = ReadInt(reader, "Doctors"),
            TodayAppointments = ReadInt(reader, "TodayAppointments"),
            Inpatients = ReadInt(reader, "Inpatients"),
            AvailableBeds = ReadInt(reader, "AvailableBeds"),
            OccupiedBeds = ReadInt(reader, "OccupiedBeds"),
            DailyRevenue = ReadDecimal(reader, "DailyRevenue"),
            OutstandingInvoices = ReadDecimal(reader, "OutstandingInvoices")
        };
    }

    public async Task<IReadOnlyList<UpcomingAppointment>> GetUpcomingAppointmentsAsync(int limit = 8, CancellationToken cancellationToken = default)
    {
        var appointments = new List<UpcomingAppointment>();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand("Dashboard_GetUpcomingAppointments", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add("@Limit", SqlDbType.Int).Value = Math.Clamp(limit, 1, 50);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            appointments.Add(new UpcomingAppointment
            {
                AppointmentId = ReadInt(reader, "AppointmentId"),
                PatientName = ReadString(reader, "PatientName"),
                DoctorName = ReadString(reader, "DoctorName"),
                Department = ReadString(reader, "Department"),
                AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                Status = ReadString(reader, "Status")
            });
        }

        return appointments;
    }

    private static int ReadInt(SqlDataReader reader, string name) =>
        reader[name] == DBNull.Value ? 0 : Convert.ToInt32(reader[name]);

    private static decimal ReadDecimal(SqlDataReader reader, string name) =>
        reader[name] == DBNull.Value ? 0m : Convert.ToDecimal(reader[name]);

    private static string ReadString(SqlDataReader reader, string name) =>
        reader[name] == DBNull.Value ? string.Empty : Convert.ToString(reader[name]) ?? string.Empty;
}
