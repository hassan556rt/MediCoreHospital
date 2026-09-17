CREATE OR ALTER PROCEDURE Pharmacy_GetLowStock
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.StockId, m.MedicineName, m.Category, s.Quantity, s.Unit, s.UnitPrice, s.ExpiryDate
    FROM MedicineStock s
    INNER JOIN Medicines m ON m.MedicineId = s.MedicineId
    WHERE s.Quantity <= 10 OR s.ExpiryDate <= DATEADD(MONTH, 3, GETDATE())
    ORDER BY s.Quantity ASC;
END;
GO

CREATE OR ALTER PROCEDURE Pharmacy_GetPrescriptions
    @SearchTerm NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PrescriptionId, CONCAT(pt.FirstName, N' ', pt.LastName) AS PatientName,
           e.FullName AS DoctorName, m.MedicineName, p.Dosage, p.Quantity, p.PrescriptionDate
    FROM Prescriptions p
    INNER JOIN Patients pt ON pt.PatientId = p.PatientId
    INNER JOIN Doctors d ON d.DoctorId = p.DoctorId
    INNER JOIN Employees e ON e.EmployeeId = d.EmployeeId
    INNER JOIN Medicines m ON m.MedicineId = p.MedicineId
    WHERE @SearchTerm IS NULL OR p.Dosage LIKE N'%' + @SearchTerm + N'%' OR m.MedicineName LIKE N'%' + @SearchTerm + N'%' OR e.FullName LIKE N'%' + @SearchTerm + N'%' OR CONCAT(pt.FirstName, N' ', pt.LastName) LIKE N'%' + @SearchTerm + N'%'
    ORDER BY p.PrescriptionDate DESC;
END;
GO
