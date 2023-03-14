namespace FirelloProject.Models
{
    public class ProductImage
    {
        public int ID { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductID { get; set; }
        public Product? Product { get; set; }
    }
}
