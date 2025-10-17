using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public ulong UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Product> Products { get; set; }=new List<Product>();
        public decimal TotalOrder { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public int? Rating { get; set; }
        public string? Status { get; set; }
    }
}
