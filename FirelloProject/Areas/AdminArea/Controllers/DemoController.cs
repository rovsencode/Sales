using FirelloProject.DAL;
using FirelloProject.Extentions;
using FirelloProject.Models;
using FirelloProject.Models.demo;
using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirelloProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DemoController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public DemoController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Authors = new SelectList(_appDbContext.Authors.ToList(), "Id", "Name");
            ViewBag.Genres = new SelectList(_appDbContext.Genres.ToList(), "Id", "Name");
            return View();

        }
        [HttpPost]
        public IActionResult Create(BookCreateVM bookCreateVM)
        {
            ViewBag.Authors = new SelectList(_appDbContext.Authors.ToList(), "Id", "Name");
            ViewBag.Genres = new SelectList(_appDbContext.Genres.ToList(), "Id", "Name");
            Book newbook = new();
            List<BookGenre> bookGenres = new();
            List<BookAuthor> bookAuthors= new();
            List<BookImages> bookImages = new();
            foreach (var photo in bookCreateVM.Photos)
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
                BookImages bookImage= new();
                bookImage.ImageUrl = photo.SaveImage(_env, "img", photo.FileName);
                bookImages.Add(bookImage);


            }

            foreach (var item in bookCreateVM.GenreIds)
            {
                BookGenre bookGenre = new();
                bookGenre.BookId = newbook.Id;
                bookGenre.GenreId = item;
                bookGenres.Add(bookGenre);
            }
            foreach (var item in bookCreateVM.AuthorIds)
            {
                BookAuthor bookAuthor = new();
                bookAuthor.BookId = newbook.Id;
                bookAuthor.AuthorId = item;
                bookAuthors.Add(bookAuthor);


            }
         
            newbook.Name=bookCreateVM.Name;
            newbook.BookGenres =bookGenres;
            newbook.BookAuthors =bookAuthors;
            newbook.BookImages =bookImages;
            _appDbContext.Add(newbook);
            _appDbContext.SaveChanges();
            return View();

        }
    }
}
