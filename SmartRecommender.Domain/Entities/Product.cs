using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<User> FavoriteByUsers { get; set; } = new List<User>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public double? AverageRating { get; set; }
        public int PopularityScore { get; set; }



    }
}
