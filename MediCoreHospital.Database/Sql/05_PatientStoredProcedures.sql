CREATE OR ALTER PROCEDURE Patient_Search
    @SearchTerm NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (200)
        PatientId, MedicalRecordNumber, NationalId, FirstName, LastName,
        Gender, DateOfBirth, BloodType, Phone, CreatedAt
    FROM Patients
    WHERE IsDeleted = 0
      AND (
          @SearchTerm IS NULL OR @SearchTerm = N''
          OR FirstName LIKE N'%' + @SearchTerm + N'%'
          OR LastName LIKE N'%' + @SearchTerm + N'%'
          OR CONCAT(FirstName, N' ', LastName) LIKE N'%' + @SearchTerm + N'%'
          OR MedicalRecordNumber LIKE N'%' + @SearchTerm + N'%'
          OR NationalId LIKE N'%' + @SearchTerm + N'%'
          OR Phone LIKE N'%' + @SearchTerm + N'%'
      )
    ORDER BY CreatedAt DESC;
END;
GO

CREATE OR ALTER PROCEDURE Patient_Create
    @MedicalRecordNumber NVARCHAR(50),
    @NationalId NVARCHAR(20) = NULL,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Gender NVARCHAR(20) = NULL,
    @DateOfBirth DATE = NULL,
    @BloodType NVARCHAR(10) = NULL,
    @Phone NVARCHAR(30) = NULL,
    @Email NVARCHAR(150) = NULL,
    @Address NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Patients WHERE MedicalRecordNumber = @MedicalRecordNumber AND IsDeleted = 0)
            THROW 51001, 'Medical record number already exists.', 1;

        INSERT INTO Patients
        (MedicalRecordNumber, NationalId, FirstName, LastName, Gender, DateOfBirth, BloodType, Phone, Email, Address)
        VALUES
        (@MedicalRecordNumber, @NationalId, @FirstName, @LastName, @Gender, @DateOfBirth, @BloodType, @Phone, @Email, @Address);

        SELECT CONVERT(INT, SCOPE_IDENTITY());
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
