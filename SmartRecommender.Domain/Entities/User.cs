using SmartRecommender.Domain.Entities;


namespace SmartRecommender.Domain.Entities
{
    public class User
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName {  get; set; }
        public string Password { get; set; }
      public  ICollection<Order> Orders { get; set; }= new List<Order>();
        public  ICollection<Product> FavoriteProducts { get; set; }=new List<Product>();


    }
}
