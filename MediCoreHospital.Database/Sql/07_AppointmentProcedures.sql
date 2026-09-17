CREATE OR ALTER PROCEDURE Appointment_GetByDate
    @AppointmentDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.AppointmentId, a.PatientId, a.DoctorId,
           CONCAT(p.FirstName, N' ', p.LastName) AS PatientName,
           e.FullName AS DoctorName,
           COALESCE(d.DepartmentName, N'غير محدد') AS DepartmentName,
           a.AppointmentDate, a.StartTime, a.EndTime, a.Status, COALESCE(a.Reason, N'') AS Reason
    FROM Appointments a
    INNER JOIN Patients p ON p.PatientId = a.PatientId
    INNER JOIN Doctors dr ON dr.DoctorId = a.DoctorId
    INNER JOIN Employees e ON e.EmployeeId = dr.EmployeeId
    LEFT JOIN Departments d ON d.DepartmentId = e.DepartmentId
    WHERE a.AppointmentDate = @AppointmentDate AND p.IsDeleted = 0
    ORDER BY a.StartTime;
END;
GO

CREATE OR ALTER PROCEDURE Appointment_Create
    @PatientId INT, @DoctorId INT, @AppointmentDate DATE,
    @StartTime TIME, @EndTime TIME, @Reason NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @EndTime <= @StartTime THROW 51002, 'Appointment end time must be after start time.', 1;
    IF EXISTS (SELECT 1 FROM Appointments WHERE DoctorId = @DoctorId AND AppointmentDate = @AppointmentDate AND Status NOT IN ('Cancelled') AND StartTime < @EndTime AND EndTime > @StartTime)
        THROW 51003, 'Doctor already has an appointment in this time range.', 1;
    INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, StartTime, EndTime, Status, Reason)
    VALUES (@PatientId, @DoctorId, @AppointmentDate, @StartTime, @EndTime, N'Scheduled', @Reason);
    SELECT CONVERT(INT, SCOPE_IDENTITY());
END;
GO

CREATE OR ALTER PROCEDURE Appointment_Cancel @AppointmentId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Appointments SET Status = N'Cancelled' WHERE AppointmentId = @AppointmentId AND Status NOT IN (N'Completed', N'Cancelled');
END;
GO
