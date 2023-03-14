using FirelloProject.DAL;
using FirelloProject.Models;
using FirelloProject.Services.Product;
using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FirelloProject.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BasketController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            //HttpContext.Session.SetString("name", "Ibrahim");
            //Response.Cookies.Append("name", "Rovsen",new CookieOptions {MaxAge=TimeSpan.FromDays(1)});
            return Content("set olundu");
        }

        public  async Task<IActionResult> Add(int id,string name) {
            if (id == null)return NotFound();
            Product product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();
            List<BasketVM> products;
            if (Request.Cookies["basket"]==null)
            {
                products = new();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            BasketVM existproduct=products.FirstOrDefault(p=>p.ID==id);
            if (existproduct == null)
            {
               
                BasketVM basketVM = new();
                basketVM.ID =product.ID;
                basketVM.BasketCount = 1;
                products.Add(basketVM);
            }
            else
            {
                existproduct.BasketCount++;
            }
         
            Response.Cookies.Append("basket",
                JsonConvert.SerializeObject(products),new CookieOptions {MaxAge=TimeSpan.FromHours(1)});
            return RedirectToAction(nameof(Index),"Home");
           
        }
        public  async Task<IActionResult> Delete(int id)
        {
            if (id == null) return NotFound();
            Product product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();
           List<Product> products =  JsonConvert.DeserializeObject<List<Product>>(Request.Cookies["basket"]);
            products.Remove(product);
            Response.Cookies.Append("basket",
              JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromHours(1) });
            return RedirectToAction("showbasket", "basket");

        }
             

        public IActionResult ShowBasket()
        {
            List<BasketVM> products;
            string basket = Request.Cookies["basket"];
            if (basket==null)
            {
                products = new();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                foreach (var item in products)
                {
                    Product currentProduct = _appDbContext.Products.Include(p => p.ProductImages)
                .FirstOrDefault(p =>p.ID==item.ID); 
                    item.Name = currentProduct.Name;
                    item.Price = currentProduct.Price;
                    item.ID= currentProduct.ID;
                    item.ImageUrl=currentProduct.ProductImages.FirstOrDefault().ImageUrl;
                }
            }
           
            return View(products);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Sale()
        {
            if (User.Identity.IsAuthenticated)
            {

            }
            else
            {
                return RedirectToAction("login", "account");
            }
            return View();
        }
      
    }
}
