using FirelloProject.DAL;
using FirelloProject.Models;
using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirelloProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SaleController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public SaleController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var sales = _appDbContext.Sales.Include(s => s.SalesProducts)
                .ThenInclude(sp=>sp.Product)
                .ToList();
            List<SaleVM> saleVMs = new();
            foreach (var item in sales)
            {
                SaleVM saleVM = new();
                AppUser user = await _userManager.FindByIdAsync(item.AppUserId);
                saleVM.SaleDate = item.CreatedDate;
                saleVM.User = user;
                saleVM.SalesProducts = item.SalesProducts;
                saleVMs.Add(saleVM);
            }
            return View(saleVMs);
        }
    }
}
