using MediCoreHospital.Application.DTOs;
using MediCoreHospital.Domain.Entities;
using Xunit;

namespace MediCoreHospital.Tests;

public sealed class PatientManagementTests
{
    [Fact]
    public void PatientDto_ComposesFullName()
    {
        var patient = new PatientDto { FirstName = "أحمد", LastName = "علي" };
        Assert.Equal("أحمد علي", patient.FullName);
    }

    [Fact]
    public void Patient_DefaultsToActiveRecord()
    {
        var patient = new Patient();
        Assert.False(patient.IsDeleted);
        Assert.Equal(string.Empty, patient.MedicalRecordNumber);
    }

    [Fact]
    public void CreatePatientRequest_PreservesOptionalValues()
    {
        var request = new CreatePatientRequest { FirstName = "Sara", LastName = "Hassan", DateOfBirth = new DateTime(1990, 1, 2), Phone = "0500000000" };
        Assert.Equal(new DateTime(1990, 1, 2), request.DateOfBirth);
        Assert.Equal("0500000000", request.Phone);
    }
}
