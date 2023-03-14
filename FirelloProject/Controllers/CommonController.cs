using FirelloProject.DAL;
using Microsoft.AspNetCore.Mvc;

namespace FirelloProject.Controllers
{
    public class CommonController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CommonController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Search(string search)
        {
            var products = _appDbContext.Products
                .Where(p => p.Name.ToLower().Contains(search.ToLower())).Take(2).OrderByDescending(p => p.ID).ToList();

            return PartialView("_SearchPartial",products);
        }
    }
}
