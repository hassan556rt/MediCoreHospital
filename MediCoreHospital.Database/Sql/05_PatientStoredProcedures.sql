CREATE OR ALTER PROCEDURE Patient_GetById
    @PatientId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PatientId, MedicalRecordNumber, NationalId, FirstName, LastName, Gender, DateOfBirth, BloodType, Phone, Email, Address, CreatedAt
    FROM Patients WHERE PatientId = @PatientId AND IsDeleted = 0;
END;
GO

CREATE OR ALTER PROCEDURE Patient_Update
    @PatientId INT,
    @MedicalRecordNumber NVARCHAR(50), @NationalId NVARCHAR(20) = NULL,
    @FirstName NVARCHAR(100), @LastName NVARCHAR(100), @Gender NVARCHAR(20) = NULL,
    @DateOfBirth DATE = NULL, @BloodType NVARCHAR(10) = NULL, @Phone NVARCHAR(30) = NULL,
    @Email NVARCHAR(150) = NULL, @Address NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF EXISTS (SELECT 1 FROM Patients WHERE MedicalRecordNumber = @MedicalRecordNumber AND PatientId <> @PatientId AND IsDeleted = 0)
        THROW 51001, 'Medical record number already exists.', 1;
    UPDATE Patients SET MedicalRecordNumber=@MedicalRecordNumber, NationalId=@NationalId, FirstName=@FirstName, LastName=@LastName,
        Gender=@Gender, DateOfBirth=@DateOfBirth, BloodType=@BloodType, Phone=@Phone, Email=@Email, Address=@Address, UpdatedAt=GETDATE()
    WHERE PatientId=@PatientId AND IsDeleted=0;
    IF @@ROWCOUNT = 0 THROW 51002, 'Patient was not found.', 1;
END;
GO

CREATE OR ALTER PROCEDURE Patient_Delete
    @PatientId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Patients SET IsDeleted=1, UpdatedAt=GETDATE() WHERE PatientId=@PatientId AND IsDeleted=0;
    IF @@ROWCOUNT = 0 THROW 51002, 'Patient was not found.', 1;
END;
GO
