CREATE OR ALTER PROCEDURE Dashboard_GetStatistics
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        (SELECT COUNT(*) FROM Patients WHERE IsDeleted = 0) AS TotalPatients,
        (SELECT COUNT(*) FROM Doctors WHERE IsActive = 1) AS Doctors,
        (SELECT COUNT(*) FROM Appointments WHERE AppointmentDate = CONVERT(date, GETDATE()) AND Status NOT IN ('Cancelled')) AS TodayAppointments,
        0 AS Inpatients,
        0 AS AvailableBeds,
        0 AS OccupiedBeds,
        CAST(0 AS DECIMAL(18,2)) AS DailyRevenue,
        CAST(0 AS DECIMAL(18,2)) AS OutstandingInvoices;
END;
GO

CREATE OR ALTER PROCEDURE Dashboard_GetUpcomingAppointments
    @Limit INT = 8
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Limit)
        a.AppointmentId,
        CONCAT(p.FirstName, N' ', p.LastName) AS PatientName,
        e.FullName AS DoctorName,
        COALESCE(d.DepartmentName, N'غير محدد') AS Department,
        a.AppointmentDate,
        a.StartTime,
        a.Status
    FROM Appointments a
    INNER JOIN Patients p ON p.PatientId = a.PatientId
    INNER JOIN Doctors dr ON dr.DoctorId = a.DoctorId
    INNER JOIN Employees e ON e.EmployeeId = dr.EmployeeId
    LEFT JOIN Departments d ON d.DepartmentId = e.DepartmentId
    WHERE a.AppointmentDate >= CONVERT(date, GETDATE())
    ORDER BY a.AppointmentDate, a.StartTime;
END;
GO
