using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace FirelloProject.Services.Basket
{
    public class BasketProductCount : IBasketProductCount
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketProductCount(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int CalculateBasketProductCount()
        {
            var basket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            if (basket == null) return 0;
            var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            return products.Sum(p => p.BasketCount);

       





        }

      
    }
    }

