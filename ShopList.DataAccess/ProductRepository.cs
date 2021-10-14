using ShopList.Infrastructure.Database;
using ShopList.Infrastructure.Model;
using ShopList.Infrastructure.Repositories;

namespace ShopList.DataAccess
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationContext context) : base(context)
        {
        }
    }
}