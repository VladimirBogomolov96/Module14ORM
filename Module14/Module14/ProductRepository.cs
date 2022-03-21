using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module14
{
    public class ProductRepository
    {
        private readonly Module13Context context;

        public ProductRepository(Module13Context context)
        {
            this.context = context;
        }

        public async Task InsertProductAsync(Product product)
        {
            await this.context.AddAsync(product);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateProductByNameAsync(string name, Product product)
        {
            Product productToUpdate = this.context.Products.First(pr => pr.Name == name);

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Width = product.Width;
            productToUpdate.Weight = product.Weight;
            productToUpdate.Height = product.Height;
            productToUpdate.Length = product.Length;

            await this.context.SaveChangesAsync();
        }

        public async Task DeleteProductByNameAsync(string name)
        {
            Product productToDelete = this.context.Products.First(pr => pr.Name == name);
            this.context.Products.Remove(productToDelete);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAllProductsAsync()
        {
            this.context.Products.RemoveRange(context.Products);
            await this.context.SaveChangesAsync();
        }

        public IEnumerable<Product> GetProducts()
        {
            return this.context.Products.ToList();
        }
    }
}
