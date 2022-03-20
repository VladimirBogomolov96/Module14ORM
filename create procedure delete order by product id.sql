CREATE PROCEDURE DeleteOrdersByProductId
    @ProductId int
AS

Delete [Order]
WHERE ProductId = @ProductId