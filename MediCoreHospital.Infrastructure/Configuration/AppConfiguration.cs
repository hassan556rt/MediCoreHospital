using Microsoft.Extensions.Configuration;

namespace MediCoreHospital.Infrastructure.Configuration;

public sealed class AppConfiguration
{
    public AppConfiguration(IConfiguration configuration)
    {
        HospitalName = configuration["App:HospitalName"] ?? "MediCore";
        Version = configuration["App:Version"] ?? "1.0.0";
        ConnectionString = configuration.GetConnectionString("HospitalDatabase") ?? string.Empty;
    }

    public string HospitalName { get; }
    public string Version { get; }
    public string ConnectionString { get; }
}
