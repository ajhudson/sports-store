using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SportsStore.Models;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class OrdersController : ApiController
    {
        private IRepository Repository { get; set; }

        public OrdersController()
        {
            Repository = (IRepository)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IRepository));  
        }

        [HttpGet]
        [Authorize(Roles="Administrators")]
        public IEnumerable<Order> List()
        {
            return Repository.Orders;
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                IDictionary<int, Product> products = Repository.Products.Where(p => order.Lines.Select(ol => ol.ProductId).Any(id => id == p.Id)).ToDictionary(p => p.Id);
                order.TotalCost = order.Lines.Sum(ol => ol.Count * products[ol.ProductId].Price);
                await Repository.SaveOrdersAsync(order);

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrators")]
        public async Task DeleteOrder(int id)
        {
            await Repository.DeleteOrdersAsync(id);
        }
    }
}
