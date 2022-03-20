CREATE PROCEDURE GetOrdersByYear
    @Year int
AS

SELECT * 
FROM [Order]
WHERE DATEPART(YEAR, CreatedDate) = @Year