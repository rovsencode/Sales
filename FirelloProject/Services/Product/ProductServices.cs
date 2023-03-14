using FirelloProject.DAL;

namespace FirelloProject.Services.Product
{
    public class ProductServices : IProduct
    {
        private readonly AppDbContext _appDbContext;
        public ProductServices(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Models.Product> GetALLProducts()
        {
            return _appDbContext.Products.ToList();

        }
    }
}
