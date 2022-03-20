CREATE PROCEDURE DeleteOrdersByMonth
    @MonthNumber int
AS

Delete [Order]
WHERE DATEPART(month, CreatedDate) = @MonthNumber