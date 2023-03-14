using FirelloProject.DAL;
//using FirelloProject.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using FirelloProject.Models;
using Newtonsoft.Json;
using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace FirelloProject.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;


        public HeaderViewComponent(AppDbContext appDbContext,UserManager<AppUser> userManager)
        {
            _userManager= userManager;
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.FullName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user= await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.FullName= user.FullName;
            }
            Bio bio = _appDbContext.Bios.FirstOrDefault();
            return View(await Task.FromResult(bio));
        }
    }
}
