IF OBJECT_ID('Admissions', 'U') IS NULL
BEGIN
    CREATE TABLE Admissions (
        AdmissionId INT IDENTITY(1,1) PRIMARY KEY,
        PatientId INT NOT NULL,
        DoctorId INT NOT NULL,
        RoomId INT NULL,
        BedId INT NULL,
        AdmissionDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        DischargeDate DATETIME2 NULL,
        Diagnosis NVARCHAR(500) NULL,
        Notes NVARCHAR(MAX) NULL,
        Status NVARCHAR(30) NOT NULL DEFAULT N'Active',
        CONSTRAINT FK_Admissions_Patients FOREIGN KEY (PatientId) REFERENCES Patients(PatientId),
        CONSTRAINT FK_Admissions_Doctors FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId)
    );
END;
GO

IF OBJECT_ID('Rooms', 'U') IS NULL
BEGIN
    CREATE TABLE Rooms (
        RoomId INT IDENTITY(1,1) PRIMARY KEY,
        RoomNumber NVARCHAR(30) NOT NULL UNIQUE,
        RoomType NVARCHAR(50) NOT NULL DEFAULT N'عام',
        DepartmentId INT NULL,
        Floor INT NOT NULL DEFAULT 1,
        Status NVARCHAR(30) NOT NULL DEFAULT N'Available',
        CONSTRAINT FK_Rooms_Departments FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
    );
END;
GO

IF OBJECT_ID('Beds', 'U') IS NULL
BEGIN
    CREATE TABLE Beds (
        BedId INT IDENTITY(1,1) PRIMARY KEY,
        RoomId INT NOT NULL,
        BedNumber NVARCHAR(30) NOT NULL,
        Status NVARCHAR(30) NOT NULL DEFAULT N'Available',
        CONSTRAINT FK_Beds_Rooms FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId),
        CONSTRAINT UQ_Beds_Room_Bed UNIQUE (RoomId, BedNumber)
    );
END;
GO

CREATE OR ALTER PROCEDURE Admission_GetActive
AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.AdmissionId, a.PatientId,
           CONCAT(p.FirstName, N' ', p.LastName) AS PatientName,
           e.FullName AS DoctorName,
           COALESCE(r.RoomNumber, N'غير محدد') AS RoomNumber,
           COALESCE(b.BedNumber, N'غير محدد') AS BedNumber,
           a.AdmissionDate, a.DischargeDate,
           COALESCE(a.Diagnosis, N'') AS Diagnosis, a.Status
    FROM Admissions a
    INNER JOIN Patients p ON p.PatientId = a.PatientId
    INNER JOIN Doctors d ON d.DoctorId = a.DoctorId
    INNER JOIN Employees e ON e.EmployeeId = d.EmployeeId
    LEFT JOIN Rooms r ON r.RoomId = a.RoomId
    LEFT JOIN Beds b ON b.BedId = a.BedId
    WHERE a.Status = N'Active' AND p.IsDeleted = 0
    ORDER BY a.AdmissionDate DESC;
END;
GO
