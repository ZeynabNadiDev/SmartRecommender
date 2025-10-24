using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Domain.Entities.AI.Models
{
    public class FeatureVector
    {
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRating { get; set; }

        public string? UseCase { get; set; }
        public string? Target { get; set; }

        public Dictionary<string, string>? Attributes { get; set; } = new();
    }
}
