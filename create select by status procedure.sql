CREATE PROCEDURE GetOrdersByStatus
    @Status int
AS

SELECT * 
FROM [Order]
WHERE Status = @Status