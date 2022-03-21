using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module14
{
    public class OrderRepository
    {
        private readonly Module13Context context;

        public OrderRepository(Module13Context context)
        {
            this.context = context;
        }

        public async Task InsertOrderAsync(Order order)
        {
            await this.context.Orders.AddAsync(order);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateOrderByIdAsync(int id, Order order)
        {
            Order orderToUpdate = this.context.Orders.First(or => or.Id == id);
            orderToUpdate.Status = order.Status;
            orderToUpdate.CreatedDate = order.CreatedDate;
            orderToUpdate.UpdatedDate = order.UpdatedDate;
            orderToUpdate.Product = order.Product;

            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAllOrdersAsync()
        {
            this.context.Orders.RemoveRange(context.Orders);
            await this.context.SaveChangesAsync();
        }

        public IEnumerable<Order> GetOrders()
        {
            return this.context.Orders.ToList();
        }

        public IEnumerable<Order> GetOrdersByMonth(int monthNumber)
        {
            SqlParameter sqlParameter = new SqlParameter("@MonthNumber", monthNumber);
            return this.context.Orders.FromSqlRaw("GetOrdersByMonth @MonthNumber", sqlParameter).ToList();
        }

        public IEnumerable<Order> GetOrdersByStatus(OrderStatus orderStatus)
        {
            SqlParameter sqlParameter = new SqlParameter("@Status", orderStatus);
            return this.context.Orders.FromSqlRaw("GetOrdersByStatus @Status", sqlParameter).ToList();
        }

        public IEnumerable<Order> GetOrdersByYear(int year)
        {
            SqlParameter sqlParameter = new SqlParameter("@Year", year);
            return this.context.Orders.FromSqlRaw("GetOrdersByYear @Year", sqlParameter).ToList();
        }

        public IEnumerable<Order> GetOrdersByProductId(int productId)
        {
            SqlParameter sqlParameter = new SqlParameter("@ProductId", productId);
            return this.context.Orders.FromSqlRaw("GetOrdersByProductId @ProductId", sqlParameter).ToList();
        }

        public async Task DeleteOrdersByMonthAsync(int monthNumber)
        {
            SqlParameter sqlParameter = new SqlParameter("@MonthNumber", monthNumber);
            await this.context.Database.ExecuteSqlRawAsync("DeleteOrdersByMonth @MonthNumber", sqlParameter);
        }

        public async Task DeleteOrdersByStatusAsync(OrderStatus orderStatus)
        {
            SqlParameter sqlParameter = new SqlParameter("@Status", (int)orderStatus);
            await this.context.Database.ExecuteSqlRawAsync("DeleteOrdersByStatus @Status", sqlParameter);
        }

        public async Task DeleteOrdersByYearAsync(int year)
        {
            SqlParameter sqlParameter = new SqlParameter("@Year", year);
            await this.context.Database.ExecuteSqlRawAsync("DeleteOrdersByYear @Year", sqlParameter);
        }

        public async Task DeleteOrdersByProductIdAsync(int productId)
        {
            SqlParameter sqlParameter = new SqlParameter("@ProductId", productId);
            await this.context.Database.ExecuteSqlRawAsync("DeleteOrdersByProductId @ProductId", sqlParameter);
        }
    }
}
