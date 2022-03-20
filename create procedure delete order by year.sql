CREATE PROCEDURE DeleteOrdersByYear
    @Year int
AS

Delete [Order]
WHERE DATEPART(YEAR, CreatedDate) = @Year