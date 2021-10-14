using Microsoft.AspNetCore.SignalR;
using ShopList.Infrastructure.DTOs;
using ShopList.Infrastructure.Model;
using ShopList.Infrastructure.Repositories;
using ShopList.Infrastructure.Services;
using ShopList.Logic.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopList.Logic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IHubContext<ShoppingListHub> _hubContext;
        private const string ProductListUpdate = nameof(ProductListUpdate);

        public ProductService(IProductRepository productRepository, IShoppingListRepository shoppingListRepository, IHubContext<ShoppingListHub> hubContext)
        {
            _productRepository = productRepository;
            _shoppingListRepository = shoppingListRepository;
            _hubContext = hubContext;
        }

        public async Task<AddProductResponse> AddProductToShoppingList(AddProductRequest addProductRequest)
        {
            var shoppingList = await _shoppingListRepository.GetById(addProductRequest.ShoppingListId);

            if (shoppingList == null)
            {
                return new AddProductResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = "You are trying to update shopping list which is not exist"
                };
            }

            var entities = addProductRequest.Products.Select(x => new Product()
            {
                Name = x.Name,
                Price = x.Price,
                Type = x.Type,
                ShoppingListId = shoppingList.Id
            });

            var existingNames = _productRepository.Get(x => x.ShoppingListId == shoppingList.Id)
                .Select(x => x.Name)
                .ToList();

            entities = entities.Where(x => !existingNames.Contains(x.Name) && x.Price > 0 && !string.IsNullOrEmpty(x.Name) && !string.IsNullOrEmpty(x.Type));

            var resposne = await _productRepository.Insert(entities);

            var result = new AddProductResponse()
            {
                IsSuccess = true,
                Products = resposne.Select(x => new ProductDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price
                })
            };

            await _hubContext.Clients.All.SendAsync(ProductListUpdate);
            return result;
        }

        public async Task<DeletedProductResponse> DeleteProductsFromShoppingList(IEnumerable<int> addProductRequest)
        {
            if (addProductRequest == null)
            {
                return new DeletedProductResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = "Value cannot be null"
                };
            }
            var deletedIds = new List<int>();

            foreach (var id in addProductRequest)
            {
                var entity = await _productRepository.GetById(id);
                if (entity != null)
                {
                    await _productRepository.Delete(id);
                    deletedIds.Add(id);
                }
            }

            await _hubContext.Clients.All.SendAsync(ProductListUpdate);

            return new DeletedProductResponse()
            {
                IsSuccess = true,
                ProductIds = deletedIds
            };
        }
    }
}