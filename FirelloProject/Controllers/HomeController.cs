//using FirelloProject.Models;
using FirelloProject.DAL;
using FirelloProject.Models;
using FirelloProject.Services.Basket;
using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FirelloProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
       
        }

        public IActionResult Index()
        {
         
           HomeVM homeVM= new HomeVM();

            homeVM.Sliders = _appDbContext.Slider.ToList();
            homeVM.SliderDetails = _appDbContext.SliderDetails.FirstOrDefault();
            homeVM.Categories = _appDbContext.Categories.ToList();
            homeVM.Products= _appDbContext.Products.Include(p=>p.ProductImages).ToList();
                return View(homeVM);
        }

    }
}