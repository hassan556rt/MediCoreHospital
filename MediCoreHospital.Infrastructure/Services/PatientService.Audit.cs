using System.Data;
using Microsoft.Data.SqlClient;

namespace MediCoreHospital.Infrastructure.Services;

// Reserved for the audit decorator that will be added with the security module.
internal static class PatientAuditSql
{
    public static void AddAuditParameters(SqlCommand command, int? userId, string operation) { }
}
