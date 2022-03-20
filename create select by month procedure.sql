CREATE PROCEDURE GetOrdersByMonth
    @MonthNumber int
AS

SELECT * 
FROM [Order]
WHERE DATEPART(month, CreatedDate) = @MonthNumber