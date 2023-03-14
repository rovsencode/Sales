using FirelloProject.DAL;
using FirelloProject.Extentions;
using FirelloProject.Models;
using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FirelloProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_appDbContext.Products.Include(p => p.ProductImages).
                Include(p=>p.Category).ToList());
        }
        public IActionResult Create()
        {

            ViewBag.Categories= new SelectList(_appDbContext.Categories.ToList(), "ID", "Name");
            return View();
        }

        [HttpPost]

        public IActionResult Create(ProductCreateVM productCreateVM)
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();
            if (!ModelState.IsValid) return View();

            bool isExist = _appDbContext.Products.Any(p=> p.Name.ToLower() == productCreateVM.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adli  mehsul adi movcuddur");
                return View();
            }
            List<ProductImage> productImages = new();
            foreach (var photo in productCreateVM.Photos)
            {
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "only image ");
                    return View();
                }
                if (photo.CheckImageSize(500))
                {
                    ModelState.AddModelError("Photo", "olcu boyukdur ");
                    return View();
                }
                ProductImage productImage = new();
                productImage.ImageUrl = photo.SaveImage(_env,"img",photo.FileName);
                productImages.Add(productImage);


            }

            Product newproduct = new();
            newproduct.Name = productCreateVM.Name;
            newproduct.Price = productCreateVM.Price;
            newproduct.CategoryID = productCreateVM.CategoryId;
            newproduct.ProductImages = productImages;
            _appDbContext.Products.Add(newproduct);
            _appDbContext.SaveChanges();
         
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();
            Product product = _appDbContext.Products.Include(p=>p.ProductImages).Include(p=>p.Category).SingleOrDefault(p=>p.ID==id);
           if(product== null) return NotFound();
           return View(product);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "ID", "Name");
            if (id == null) return NotFound();
            Product product = _appDbContext.Products.Include(p => p.ProductImages).Include(p => p.Category).SingleOrDefault(p => p.ID == id);
            if (product == null) return NotFound();
            return View(new ProductUpdateVM { Name = product.Name, Price=product.Price,ProductImages=product.ProductImages});
        }
        [HttpPost]
        public IActionResult Edit(int id,ProductUpdateVM productUpdateVM)
        {
            
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "ID", "Name");
            if (id == null) return NotFound();
            if (!ModelState.IsValid) return View();
            Product product = _appDbContext.Products.Include(p => p.ProductImages).Include(p => p.Category).SingleOrDefault(p => p.ID == id);

            bool isExist = _appDbContext.Products.Any(p => p.Name.ToLower() == productUpdateVM.Name.ToLower() && p.ID != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adli  mehsul adi movcuddur");
                return View();
            }
            List<ProductImage> productImages = new();
            if (productUpdateVM.Photos != null)
            {
                foreach (var photo in productUpdateVM.Photos)
                {
                    if (!photo.IsImage())
                    {
                        ModelState.AddModelError("Photo", "only image ");
                        return View();
                    }
                    if (photo.CheckImageSize(500))
                    {
                        ModelState.AddModelError("Photo", "olcu boyukdur ");
                        return View();
                    }
                   
                    ProductImage productImage = new();
                    productImage.ImageUrl = photo.SaveImage(_env, "img", photo.FileName);
                    productImages.Add(productImage);
                    


                }
            }
            ProductImage productImage1 = _appDbContext.ProductImages.FirstOrDefault(p => p.ProductID == id);
            Product existProudct = _appDbContext.Products.Find(id);
            if (existProudct == null) return NotFound();
            existProudct.Name = productUpdateVM.Name;
            existProudct.Price = productUpdateVM.Price;
            existProudct.CategoryID = productUpdateVM.CategoryId;
            
            if (productImages!=null)
            {
                string fullPath = Path.Combine(_env.WebRootPath, "img", productImage1.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                existProudct.ProductImages = productImages;
            }
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
         

        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var product = _appDbContext.Products.FirstOrDefault(c => c.ID == id);
            if (product == null) return NotFound();
     
            _appDbContext.Products.Remove(product);
            ProductImage productImage = _appDbContext.ProductImages.FirstOrDefault(p => p.ProductID == id);
            if (productImage == null) return NotFound();
            string fullPath = Path.Combine(_env.WebRootPath, "img", productImage.ImageUrl);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
