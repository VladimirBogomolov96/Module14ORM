CREATE TABLE [Order]
(
	Id INT PRIMARY KEY IDENTITY,
	Status int,
	CreatedDate DATE,
	UpdatedDate DATE,
	ProductId INT NOT NULL,
	CONSTRAINT FK_Order_To_Product FOREIGN KEY(ProductId) REFERENCES Product(Id)
)