CREATE OR ALTER PROCEDURE Department_GetActive
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DepartmentId, DepartmentName, COALESCE(Description, N'') AS Description
    FROM Departments WHERE IsActive = 1 ORDER BY DepartmentName;
END;
GO

CREATE OR ALTER PROCEDURE Doctor_Search
    @SearchTerm NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dr.DoctorId, e.FullName, COALESCE(dr.Specialty, N'') AS Specialty,
           COALESCE(dr.LicenseNumber, N'') AS LicenseNumber,
           COALESCE(d.DepartmentName, N'غير محدد') AS DepartmentName,
           dr.ConsultationFee, dr.IsActive
    FROM Doctors dr
    INNER JOIN Employees e ON e.EmployeeId = dr.EmployeeId
    LEFT JOIN Departments d ON d.DepartmentId = e.DepartmentId
    WHERE dr.IsActive = 1 AND
          (@SearchTerm IS NULL OR e.FullName LIKE N'%' + @SearchTerm + N'%' OR dr.Specialty LIKE N'%' + @SearchTerm + N'%')
    ORDER BY e.FullName;
END;
GO
