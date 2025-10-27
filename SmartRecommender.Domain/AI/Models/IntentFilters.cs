using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Domain.AI.Models
{
    public class IntentFilters
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Feature { get; set; }
    }
}
