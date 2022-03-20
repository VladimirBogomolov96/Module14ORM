CREATE PROCEDURE GetOrdersByProductId
    @ProductId int
AS

SELECT * 
FROM [Order]
WHERE ProductId = @ProductId