using FirelloProject.Models;

namespace FirelloProject.ViewModels
{
    public class BasketVM
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? ImageUrl { get; set;}      
        public int BasketCount { get; set; }
    }
}
