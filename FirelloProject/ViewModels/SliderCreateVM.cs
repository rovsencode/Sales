using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;


namespace FirelloProject.ViewModels
{
    public class SliderCreateVM
    {

        [Required]
        public IFormFile? Photo { get; set; }
    }
}
