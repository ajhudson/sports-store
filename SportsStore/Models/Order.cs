using System.Collections.Generic;

namespace SportsStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Customer { get; set; }
        public decimal TotalCost { get; set; }
        public ICollection<OrderLine> Lines { get; set; }
    }
}