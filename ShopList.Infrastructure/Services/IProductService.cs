using ShopList.Infrastructure.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopList.Infrastructure.Services
{
    public interface IProductService
    {
        Task<AddProductResponse> AddProductToShoppingList(AddProductRequest addProductRequest);

        Task<DeletedProductResponse> DeleteProductsFromShoppingList(IEnumerable<int> addProductRequest);
    }
}