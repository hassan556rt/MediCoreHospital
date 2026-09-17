using System.Data;
using BCrypt.Net;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Domain.Enums;
using MediCoreHospital.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MediCoreHospital.Infrastructure.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(ISqlConnectionFactory connectionFactory, ILogger<AuthenticationService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResult
            {
                Success = false,
                Message = "يرجى إدخال اسم المستخدم وكلمة المرور."
            };
        }

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = @"
            SELECT u.UserId, u.Username, u.PasswordHash, u.FullName, r.RoleName
            FROM Users u
            INNER JOIN Roles r ON r.RoleId = u.RoleId
            WHERE u.Username = @Username AND u.IsActive = 1";

        command.Parameters.AddWithValue("@Username", request.Username.Trim());

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            _logger.LogWarning("Failed login attempt for username: {Username}", request.Username);
            return new LoginResult
            {
                Success = false,
                Message = "اسم المستخدم أو كلمة المرور غير صحيحة."
            };
        }

        var userId = reader.GetInt32(reader.GetOrdinal("UserId"));
        var storedHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
        var fullName = reader.GetString(reader.GetOrdinal("FullName"));
        var roleName = reader.GetString(reader.GetOrdinal("RoleName"));

        var passwordVerified = BCrypt.Verify(request.Password, storedHash);
        if (!passwordVerified)
        {
            _logger.LogWarning("Invalid password used for login: {Username}", request.Username);
            return new LoginResult
            {
                Success = false,
                Message = "اسم المستخدم أو كلمة المرور غير صحيحة."
            };
        }

        _logger.LogInformation("Successful login for {Username}", request.Username);

        return new LoginResult
        {
            Success = true,
            Message = "تم تسجيل الدخول بنجاح.",
            UserId = userId,
            FullName = fullName,
            Username = request.Username,
            RoleName = roleName
        };
    }
}
