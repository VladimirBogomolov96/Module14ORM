CREATE PROCEDURE DeleteOrdersByStatus
    @Status int
AS

Delete [Order]
WHERE Status = @Status