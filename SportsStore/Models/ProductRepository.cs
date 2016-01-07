using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SportsStore.Models
{
    public class ProductRepository : IRepository
    {
        private ProductDbContext context = new ProductDbContext();

        public IEnumerable<Order> Orders
        {
            get
            {
                return context.Orders.Include("Lines").Include("Lines.Product");
            }
        }

        public IEnumerable<Product> Products
        {
            get
            {
                return context.Products;
            }
        }

        public async Task<Order> DeleteOrdersAsync(int orderId)
        {
            Order dbEntry = context.Orders.Find(orderId);

            if (dbEntry != null)
            {
                context.Orders.Remove(dbEntry);
            }
            await context.SaveChangesAsync();

            return dbEntry;
        }

        public async Task<Product> DeleteProductsAsync(int productId)
        {
            Product dbEntry = context.Products.Find(productId);

            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
            }
            await context.SaveChangesAsync();

            return dbEntry;
        }

        public async Task<int> SaveOrdersAsync(Order order)
        {
            if (order.OrderId == 0)
            {
                context.Orders.Add(order);
            }

            return await context.SaveChangesAsync();
        }

        public async Task<int> SaveProductsAsync(Product product)
        {
            if (product.Id == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.Id);

                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }

            return await context.SaveChangesAsync();
        }
    }
}