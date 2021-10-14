using ShopList.Infrastructure.Database;
using ShopList.Infrastructure.Model;
using ShopList.Infrastructure.Repositories;

namespace ShopList.DataAccess
{
    public class ShoppingListRepository : GenericRepository<ShoppingList>, IShoppingListRepository
    {
        public ShoppingListRepository(ApplicationContext context) : base(context)
        {
        }
    }
}