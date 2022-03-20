using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module14.Tests
{
    [TestClass]
    public class OrderRepositoryTests
    {
        // Here should be a test database, but in this task there is no reason to create a separate one for testing
        private const string testConnectionString = "Server=(localdb)\\mssqllocaldb;Database=Module13;Trusted_Connection=True;";

        private readonly OrderRepository orderRepository = new OrderRepository();

        private readonly ProductRepository productRepository = new ProductRepository();

        private readonly Order[] initialOrders = new Order[]
        {
            new Order()
            {
                Status = OrderStatus.Loading,
                CreatedDate = new DateTime(2001, 1, 1),
                UpdatedDate = new DateTime(2002, 1, 1)
            },
            new Order()
            {
                Status = OrderStatus.Cancelled,
                CreatedDate = new DateTime(2003, 4, 7),
                UpdatedDate = new DateTime(2008, 2, 9)
            },
            new Order()
            {
                Status = OrderStatus.Arrived,
                CreatedDate = new DateTime(2022, 5, 5),
                UpdatedDate = new DateTime(2025, 4, 7)
            }
        };

        private readonly Product[] initialProducts = new Product[]
        {
            new Product(){ Name = "Meat", Description = "From France", Height = 100, Length = 200, Weight = 450, Width = 55 },
            new Product(){ Name = "Bananas", Description = "From India", Height = 109, Length = 20, Weight = 43, Width = 25 },
            new Product(){ Name = "Shoes", Description = "From China", Height = 28, Length = 240, Weight = 111, Width = 33 }
        };

        [TestInitialize]
        public async Task Setup()
        {
            foreach (Product product in this.initialProducts)
            {
                await this.productRepository.InsertProductAsync(product);
            }

            List<Product> products = this.productRepository.GetProducts();

            for (int i = 0; i < products.Count; i++)
            {
                this.initialOrders[i].ProductId = products[i].Id;
            }

            foreach (Order order in this.initialOrders)
            {
                await this.orderRepository.InsertOrderAsync(order);
            }
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            await this.orderRepository.DeleteAllOrdersAsync();
            await this.productRepository.DeleteAllProductsAsync();
        }

        [TestMethod]
        public void GetOrders_ReturnsExpectedOrders()
        {
            // Act
            List<Order> actual = this.orderRepository.GetOrders();

            // Assert
            Assert.AreEqual(this.initialOrders.Length, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.IsTrue(this.AreOrdersEqual(actual[i], this.initialOrders[i]));
            }
        }

        [TestMethod]
        public async Task InsertOrderAsync_AddsOrder()
        {
            // Arrange
            Order order = new Order()
            {
                Status = OrderStatus.InProgress,
                CreatedDate = new DateTime(2001, 1, 1),
                UpdatedDate = new DateTime(2002, 1, 1),
                ProductId = this.productRepository.GetProducts().Last().Id
            };

            // Act
            await this.orderRepository.InsertOrderAsync(order);

            // Assert
            Assert.IsTrue(this.AreOrdersEqual(order, this.GetOrders().Result.Last()));
        }

        [TestMethod]
        public async Task UpdateOrderByIdAsync_UpdatesOrder()
        {
            // Arrange
            Order order = new Order()
            {
                Status = OrderStatus.InProgress,
                CreatedDate = new DateTime(2001, 1, 1),
                UpdatedDate = new DateTime(2002, 1, 1),
                ProductId = this.productRepository.GetProducts().Last().Id
            };

            int idToUpdate = this.GetOrders().Result.Last().Id;

            // Act
            await this.orderRepository.UpdateOrderByIdAsync(idToUpdate, order);

            // Assert
            Assert.IsTrue(this.AreOrdersEqual(order, this.GetOrders().Result.Where(ord => ord.Id == idToUpdate).First()));
        }

        [TestMethod]
        public async Task DeleteAllOrdersAsync_DeletesAllOrders()
        {
            // Act
            await this.orderRepository.DeleteAllOrdersAsync();

            // Assert
            Assert.AreEqual(0, this.GetOrders().Result.Count);
        }

        [TestMethod]
        [DataRow(10, 0)]
        [DataRow(1, 1)]
        [DataRow(4, 1)]
        public void GetOrdersByMonth_ReturnsCorrectOrders(int monthNumber, int expectedOrdersNumber)
        {
            // Act
            List<Order> orders = this.orderRepository.GetOrdersByMonth(monthNumber);

            // Assert
            Assert.AreEqual(expectedOrdersNumber, orders.Count);
        }

        [TestMethod]
        [DataRow(OrderStatus.Done, 0)]
        [DataRow(OrderStatus.Loading, 1)]
        [DataRow(OrderStatus.Cancelled, 1)]
        public void GetOrdersByStatus_ReturnsCorrectOrders(OrderStatus orderStatus, int expectedOrdersNumber)
        {
            // Act
            List<Order> orders = this.orderRepository.GetOrdersByStatus(orderStatus);

            // Assert
            Assert.AreEqual(expectedOrdersNumber, orders.Count);
        }

        [TestMethod]
        [DataRow(2010, 0)]
        [DataRow(2022, 1)]
        [DataRow(2003, 1)]
        public void GetOrdersByYear_ReturnsCorrectOrders(int year, int expectedOrdersNumber)
        {
            // Act
            List<Order> orders = this.orderRepository.GetOrdersByYear(year);

            // Assert
            Assert.AreEqual(expectedOrdersNumber, orders.Count);
        }

        [TestMethod]
        public void GetOrdersByProductId_NotExistingProductId_ReturnsZeroOrders()
        {
            // Act
            List<Order> orders = this.orderRepository.GetOrdersByProductId(-5);

            // Assert
            Assert.AreEqual(0, orders.Count);
        }

        [TestMethod]
        public void GetOrdersByProductId_ExistingProductId_ReturnsCorrectOrders()
        {
            // Arrange
            Order order = this.GetOrders().Result.Last();

            // Act
            List<Order> orders = this.orderRepository.GetOrdersByProductId(order.ProductId);

            // Assert
            Assert.IsTrue(orders.Any(ord => this.AreOrdersEqual(ord, order)));
        }

        [TestMethod]
        [DataRow(10, 3)]
        [DataRow(1, 2)]
        [DataRow(4, 2)]
        public async Task DeleteOrdersByMonthAsync_DeletesCorrectOrders(int monthNumber, int expectedOrdersLeftNumber)
        {
            // Act
            await this.orderRepository.DeleteOrdersByMonthAsync(monthNumber);

            // Assert
            Assert.AreEqual(expectedOrdersLeftNumber, this.GetOrders().Result.Count);
        }

        [TestMethod]
        [DataRow(OrderStatus.Done, 3)]
        [DataRow(OrderStatus.Loading, 2)]
        [DataRow(OrderStatus.Cancelled, 2)]
        public async Task DeleteOrdersByStatusAsync_DeletesCorrectOrders(OrderStatus orderStatus, int expectedOrdersLeftNumber)
        {
            // Act
            await this.orderRepository.DeleteOrdersByStatusAsync(orderStatus);

            // Assert
            Assert.AreEqual(expectedOrdersLeftNumber, this.GetOrders().Result.Count);
        }

        [TestMethod]
        [DataRow(2010, 3)]
        [DataRow(2022, 2)]
        [DataRow(2003, 2)]
        public async Task DeleteOrdersByYearAsync_DeletesCorrectOrders(int year, int expectedOrdersLeftNumber)
        {
            // Act
            await this.orderRepository.DeleteOrdersByYearAsync(year);

            // Assert
            Assert.AreEqual(expectedOrdersLeftNumber, this.GetOrders().Result.Count);
        }

        [TestMethod]
        public async Task DeleteOrdersByProductIdAsync_NotExistingProductId_DeletesZeroOrders()
        {
            // Act
            await this.orderRepository.DeleteOrdersByProductIdAsync(-5);

            // Assert
            Assert.AreEqual(3, this.GetOrders().Result.Count);
        }

        [TestMethod]
        public async Task DeleteOrdersByProductIdAsync_ExistingProductId_DeletesCorrectOrders()
        {
            // Arrange
            Order order = this.GetOrders().Result.Last();

            // Act
            await this.orderRepository.DeleteOrdersByProductIdAsync(order.ProductId);

            // Assert
            Assert.IsFalse(this.GetOrders().Result.Any(ord => this.AreOrdersEqual(ord, order)));
        }

        private async Task<List<Order>> GetOrders()
        {
            string query = "SELECT * FROM [Order]";

            using SqlConnection connection = new SqlConnection(OrderRepositoryTests.testConnectionString);

            await connection.OpenAsync();

            SqlCommand command = new SqlCommand(query, connection);

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            List<Order> result = new List<Order>();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    Order order = new Order()
                    {
                        Id = (int)reader["Id"],
                        Status = (OrderStatus)reader["Status"],
                        CreatedDate = (DateTime)reader["CreatedDate"],
                        UpdatedDate = (DateTime)reader["UpdatedDate"],
                        ProductId = (int)reader["ProductId"]
                    };

                    result.Add(order);
                }
            }

            return result;
        }

        private bool AreOrdersEqual(Order lhs, Order rhs)
        {
            return lhs.Status == rhs.Status &&
                lhs.CreatedDate == rhs.CreatedDate &&
                lhs.UpdatedDate == rhs.UpdatedDate &&
                lhs.ProductId == rhs.ProductId;
        }
    }
}
