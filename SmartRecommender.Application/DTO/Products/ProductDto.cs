namespace SmartRecommender.Application.Products.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public decimal? Discount { get; set; }

        public string? Category { get; set; }

        public double? AverageRating { get; set; }
        public int PopularityScore { get; set; }
        public decimal FinalPrice => Discount.HasValue
            ? Price - (Price * Discount.Value / 100)
            : Price;
      
        public Dictionary<string, string>? Attributes { get; set; }
        public string? Description { get; set; }
    }
}
