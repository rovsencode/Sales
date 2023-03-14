using FirelloProject.DAL;
using FirelloProject.Models;
using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FirelloProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View(_appDbContext.Categories.ToList());

        }

        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();       
            Category category=_appDbContext.Categories.SingleOrDefault(c=>c.ID==id);
            if (category == null) return NotFound();
            return View(category);
        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid) return View();

            bool isExist=_appDbContext.Categories.Any(c=>c.Name.ToLower()==category.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu category  movcuddur");
                return View();
            }
            

            Category newCategory = new() { 
            Name= category.Name,
            Description= category.Description,
            };

            _appDbContext.Categories.Add(newCategory);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            Category category = _appDbContext.Categories.SingleOrDefault(c => c.ID == id);
            if (category == null) return NotFound();
            return View(new CategoryUpdateVM {Name=category.Name,Description=category.Description });
        }
        [HttpPost]
        public IActionResult Edit(int id,CategoryUpdateVM updateVM)
        {
            if (id == null) return NotFound();
            if (!ModelState.IsValid) return View();

 
            bool isExist = _appDbContext.Categories.Any(c => c.Name.ToLower() == updateVM.Name.ToLower() && c.ID !=id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu category  movcuddur");
                return View();
            }

            Category existCategory = _appDbContext.Categories.Find(id);
            if (existCategory == null) return NotFound();
            existCategory.Name = updateVM.Name;
            existCategory.Description = updateVM.Description;
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Category category = _appDbContext.Categories.SingleOrDefault(c => c.ID == id);
            if (category == null) return NotFound();
            _appDbContext.Categories.Remove(category);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
   
   
}
