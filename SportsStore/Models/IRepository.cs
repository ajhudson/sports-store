using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    interface IRepository
    {
        IEnumerable<Product> Products { get; }
        Task<int> SaveProductsAsync(Product product);
        Task<Product> DeleteProductsAsync(int productId);
        IEnumerable<Order> Orders { get; }
        Task<int> SaveOrdersAsync(Order order);
        Task<Order> DeleteOrdersAsync(int orderId);
    }
}
