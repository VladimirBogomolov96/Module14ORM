using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module14
{
    public class ProductRepository
    {
        public async Task InsertProductAsync(Product product)
        {
            using Module13Context context = new Module13Context();
            await context.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public async Task UpdateProductByNameAsync(string name, Product product)
        {
            using Module13Context context = new Module13Context();
            Product productToUpdate = context.Products.First(pr => pr.Name == name);

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Width = product.Width;
            productToUpdate.Weight = product.Weight;
            productToUpdate.Height = product.Height;
            productToUpdate.Length = product.Length;

            await context.SaveChangesAsync();
        }

        public async Task DeleteProductByNameAsync(string name)
        {
            using Module13Context context = new Module13Context();
            Product productToDelete = context.Products.First(pr => pr.Name == name);
            context.Products.Remove(productToDelete);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAllProductsAsync()
        {
            using Module13Context context = new Module13Context();
            context.Products.RemoveRange(context.Products);
            await context.SaveChangesAsync();
        }

        public List<Product> GetProducts()
        {
            using Module13Context context = new Module13Context();
            return context.Products.ToList();
        }
    }
}
