using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace FirelloProject.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required,MaxLength(50)]
        public string? Name { get; set; }
        [Required, MinLength(5)]
        public string? Description { get; set; }
        public List<Product>? Products { get; set; }

    }
}
