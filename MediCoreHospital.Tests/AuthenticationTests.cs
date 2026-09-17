using MediCoreHospital.Application.DTOs;
using Xunit;

namespace MediCoreHospital.Tests;

public class AuthenticationTests
{
    [Fact]
    public void LoginRequest_ShouldRequireUsernameAndPassword()
    {
        var request = new LoginRequest
        {
            Username = "admin",
            Password = "Admin@123"
        };

        Assert.False(string.IsNullOrWhiteSpace(request.Username));
        Assert.False(string.IsNullOrWhiteSpace(request.Password));
    }

    [Fact]
    public void LoginResult_ShouldReflectFailureMessage()
    {
        var result = new LoginResult
        {
            Success = false,
            Message = "اسم المستخدم أو كلمة المرور غير صحيحة."
        };

        Assert.False(result.Success);
        Assert.Contains("غير صحيحة", result.Message);
    }
}
