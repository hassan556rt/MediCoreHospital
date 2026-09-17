CREATE OR ALTER PROCEDURE Invoice_GetOpen
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.InvoiceId, i.InvoiceNumber, i.IssueDate, i.TotalAmount, i.PaidAmount, i.Status,
           CONCAT(p.FirstName, N' ', p.LastName) AS PatientName
    FROM Invoices i
    INNER JOIN Patients p ON p.PatientId = i.PatientId
    WHERE i.Status IN (N'Open', N'Partial')
    ORDER BY i.IssueDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE Invoice_Create
    @PatientId INT,
    @InvoiceNumber NVARCHAR(50),
    @TotalAmount DECIMAL(18,2),
    @PaidAmount DECIMAL(18,2) = 0,
    @Status NVARCHAR(30) = N'Open'
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Invoices WHERE InvoiceNumber = @InvoiceNumber)
        THROW 51005, 'Invoice number already exists.', 1;

    INSERT INTO Invoices (PatientId, InvoiceNumber, IssueDate, TotalAmount, PaidAmount, Status)
    VALUES (@PatientId, @InvoiceNumber, GETDATE(), @TotalAmount, @PaidAmount, @Status);

    SELECT CONVERT(INT, SCOPE_IDENTITY());
END;
GO
