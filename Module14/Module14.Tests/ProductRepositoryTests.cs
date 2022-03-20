using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module14.Tests
{
    [TestClass]
    public class ProductRepositoryTests
    {
        // Here should be a test database, but in this task there is no reason to create a separate one for testing
        private const string testConnectionString = "Server=(localdb)\\mssqllocaldb;Database=Module13;Trusted_Connection=True;";

        private readonly ProductRepository productRepository = new ProductRepository();

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
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            await this.productRepository.DeleteAllProductsAsync();
        }

        [TestMethod]
        public void GetProducts_ReturnsExpectedProducts()
        {
            // Act
            List<Product> actual = this.productRepository.GetProducts();

            // Assert
            Assert.AreEqual(this.initialProducts.Length, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.IsTrue(this.AreProductsEqual(actual[i], this.initialProducts[i]));
            }
        }

        [TestMethod]
        public async Task InsertProductAsync_AddsProduct()
        {
            // Arrange
            Product product = new Product() { Name = "TestProduct", Description = "TestDescription", Height = 0, Length = 0, Weight = 0, Width = 0 };

            // Act
            await this.productRepository.InsertProductAsync(product);
            Product actual = this.GetProducts().Result.Last();

            // Assert
            Assert.IsTrue(this.AreProductsEqual(actual, product));
        }

        [TestMethod]
        public async Task UpdateProductByNameAsync_UpdatesProduct()
        {
            // Arrange
            Product product = new Product() { Name = "TestProduct", Description = "TestDescription", Height = 0, Length = 0, Weight = 0, Width = 0 };
            string nameToUpdate = "Meat";

            // Act
            await this.productRepository.UpdateProductByNameAsync(nameToUpdate, product);
            Product actual = this.GetProducts().Result.Where(pr => pr.Name == product.Name).First();

            // Assert
            Assert.IsTrue(this.AreProductsEqual(actual, product));
        }

        [TestMethod]
        public async Task DeleteProductByNameAsync_DeletesProduct()
        {
            // Arrange
            string nameToDelete = "Meat";

            // Act
            await this.productRepository.DeleteProductByNameAsync(nameToDelete);

            // Assert
            Assert.AreEqual(0, this.GetProducts().Result.Where(pr => pr.Name == nameToDelete).Count());
        }

        [TestMethod]
        public async Task DeleteAllProductsAsync_DeletesAllProducts()
        {
            // Act
            await this.productRepository.DeleteAllProductsAsync();

            // Assert
            Assert.AreEqual(0, this.GetProducts().Result.Count());
        }

        private async Task<List<Product>> GetProducts()
        {
            string query = "SELECT * FROM Product";

            using SqlConnection connection = new SqlConnection(ProductRepositoryTests.testConnectionString);

            await connection.OpenAsync();

            SqlCommand command = new SqlCommand(query, connection);

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            List<Product> result = new List<Product>();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    Product product = new Product()
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Description = (string)reader["Description"],
                        Weight = (int)reader["Weight"],
                        Height = (int)reader["Height"],
                        Width = (int)reader["Width"],
                        Length = (int)reader["Length"]
                    };

                    result.Add(product);
                }
            }

            return result;
        }

        private bool AreProductsEqual(Product lhs, Product rhs)
        {
            return lhs.Name == rhs.Name &&
                lhs.Description == rhs.Description &&
                lhs.Weight == rhs.Weight &&
                lhs.Height == rhs.Height &&
                lhs.Width == rhs.Width &&
                lhs.Length == rhs.Length;
        }
    }
}
