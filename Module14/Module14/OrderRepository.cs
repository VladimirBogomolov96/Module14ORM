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
        public async Task InsertOrderAsync(Order order)
        {
            using Module13Context context = new Module13Context();
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }

        public async Task UpdateOrderByIdAsync(int id, Order order)
        {
            using Module13Context context = new Module13Context();

            Order orderToUpdate = context.Orders.First(or => or.Id == id);
            orderToUpdate.Status = order.Status;
            orderToUpdate.CreatedDate = order.CreatedDate;
            orderToUpdate.UpdatedDate = order.UpdatedDate;
            orderToUpdate.Product = order.Product;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAllOrdersAsync()
        {
            using Module13Context context = new Module13Context();
            context.Orders.RemoveRange(context.Orders);
            await context.SaveChangesAsync();
        }

        public List<Order> GetOrders()
        {
            using Module13Context context = new Module13Context();
            return context.Orders.ToList();
        }

        public List<Order> GetOrdersByMonth(int monthNumber)
        {
            using Module13Context context = new Module13Context();
            SqlParameter sqlParameter = new SqlParameter("@MonthNumber", monthNumber);
            return context.Orders.FromSqlRaw("GetOrdersByMonth @MonthNumber", sqlParameter).ToList();
        }

        public List<Order> GetOrdersByStatus(OrderStatus orderStatus)
        {
            using Module13Context context = new Module13Context();
            SqlParameter sqlParameter = new SqlParameter("@Status", orderStatus);
            return context.Orders.FromSqlRaw("GetOrdersByStatus @Status", sqlParameter).ToList();
        }

        public List<Order> GetOrdersByYear(int year)
        {
            using Module13Context context = new Module13Context();
            SqlParameter sqlParameter = new SqlParameter("@Year", year);
            return context.Orders.FromSqlRaw("GetOrdersByYear @Year", sqlParameter).ToList();
        }

        public List<Order> GetOrdersByProductId(int productId)
        {
            using Module13Context context = new Module13Context();
            SqlParameter sqlParameter = new SqlParameter("@ProductId", productId);
            return context.Orders.FromSqlRaw("GetOrdersByProductId @ProductId", sqlParameter).ToList();
        }

        public async Task DeleteOrdersByMonthAsync(int monthNumber)
        {
            using Module13Context context = new Module13Context();
            SqlParameter sqlParameter = new SqlParameter("@MonthNumber", monthNumber);
            await context.Database.ExecuteSqlRawAsync("DeleteOrdersByMonth @MonthNumber", sqlParameter);
        }

        public async Task DeleteOrdersByStatusAsync(OrderStatus orderStatus)
        {
            using Module13Context context = new Module13Context();
            SqlParameter sqlParameter = new SqlParameter("@Status", (int)orderStatus);
            await context.Database.ExecuteSqlRawAsync("DeleteOrdersByStatus @Status", sqlParameter);
        }

        public async Task DeleteOrdersByYearAsync(int year)
        {
            using Module13Context context = new Module13Context();
            SqlParameter sqlParameter = new SqlParameter("@Year", year);
            await context.Database.ExecuteSqlRawAsync("DeleteOrdersByYear @Year", sqlParameter);
        }

        public async Task DeleteOrdersByProductIdAsync(int productId)
        {
            using Module13Context context = new Module13Context();
            SqlParameter sqlParameter = new SqlParameter("@ProductId", productId);
            await context.Database.ExecuteSqlRawAsync("DeleteOrdersByProductId @ProductId", sqlParameter);
        }
    }
}
