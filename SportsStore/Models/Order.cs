using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace SportsStore.Models
{
    public class Order
    {
        [HttpBindNever]
        public int OrderId { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        [HttpBindNever]
        public decimal TotalCost { get; set; }

        public ICollection<OrderLine> Lines { get; set; }
    }
}