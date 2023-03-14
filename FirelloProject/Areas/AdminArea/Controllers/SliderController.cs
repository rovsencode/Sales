using FirelloProject.DAL;
using FirelloProject.Extentions;
using FirelloProject.Models;
using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FirelloProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;



        public SliderController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_appDbContext.Slider.ToList());
        }

        public IActionResult Create()
        {

            return View();

        }
        [HttpPost]
        public IActionResult Create(SliderCreateVM sliderCreateVM)
        {
            if (sliderCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Bosh qoyma ");
                return View();
            }
            if (!sliderCreateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "only image ");
                return View();
            }
            if (sliderCreateVM.Photo.CheckImageSize(500))
            {
                ModelState.AddModelError("Photo", "olcu boyukdur ");
                return View();

            }

         
            Slider newSlider = new();
            newSlider.ImageUrl = sliderCreateVM.Photo.SaveImage(_env,"img",sliderCreateVM.Photo.FileName);

            _appDbContext.Slider.Add(newSlider);
            _appDbContext.SaveChanges();
           return RedirectToAction("Index");

        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var slider = _appDbContext.Slider.FirstOrDefault(c => c.ID == id);
            _appDbContext.Slider.Remove(slider);
            if (slider == null) return NotFound();

            string fullPath = Path.Combine(_env.WebRootPath, "img", slider.ImageUrl);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");

            
        }
        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            Slider slider = _appDbContext.Slider.SingleOrDefault(c => c.ID == id);
            if (slider == null) return NotFound();
            return View( new SliderUpdateVM { ImageUrl=slider.ImageUrl });


        }
        [HttpPost]
        public IActionResult Edit(int id,SliderUpdateVM sliderUpdateVM)
        {
            if (id == null) return NotFound();
            Slider slider = _appDbContext.Slider.SingleOrDefault(c => c.ID == id);
            if (slider == null) return NotFound();
            if (sliderUpdateVM.Photo!=null)
            {
           
                if (!sliderUpdateVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "only image ");
                    return View();
                }
                if (sliderUpdateVM.Photo.CheckImageSize(500))
                {
                    ModelState.AddModelError("Photo", "olcu boyukdur ");
                    return View();

                }
                string fullPath = Path.Combine(_env.WebRootPath, "img", slider.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

            slider.ImageUrl = sliderUpdateVM.Photo.SaveImage(_env,"img", sliderUpdateVM.Photo.FileName);
                _appDbContext.SaveChanges();
            }
           
            return RedirectToAction("Index");
            
        }



    }
}

