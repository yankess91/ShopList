using ShopList.Infrastructure.DTOs;
using System.Threading.Tasks;

namespace ShopList.Infrastructure.Services
{
    public interface IShoppingListService
    {
        Task<CreateShoppingListResponse> CreateShoppingList(CreateShoppingListRequest createShoppingListRequest);

        Task<DeletedShoppingListResponse> DeleteShoppingList(int id);

        GetAllShoppingListResponse GetAllShoppingList();
    }
}