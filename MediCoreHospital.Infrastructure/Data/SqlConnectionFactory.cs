using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MediCoreHospital.Infrastructure.Data;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("HospitalDatabase")
            ?? throw new InvalidOperationException("Connection string 'HospitalDatabase' was not found.");
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
