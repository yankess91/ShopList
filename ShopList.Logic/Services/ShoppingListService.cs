using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using ShopList.Infrastructure.DTOs;
using ShopList.Infrastructure.Mapper;
using ShopList.Infrastructure.Model;
using ShopList.Infrastructure.Repositories;
using ShopList.Infrastructure.Services;
using ShopList.Logic.Hubs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopList.Logic.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IShoppingListMapper _shoppingListMapper;
        private readonly IHubContext<ShoppingListHub> _hubContext;
        private const string ShoppingListUpdate = nameof(ShoppingListUpdate);

        public ShoppingListService(IShoppingListRepository shoppingListRepository, IShoppingListMapper shoppingListMapper, IHubContext<ShoppingListHub> hubContext)
        {
            _shoppingListRepository = shoppingListRepository;
            _shoppingListMapper = shoppingListMapper;
            _hubContext = hubContext;
        }

        public async Task<CreateShoppingListResponse> CreateShoppingList(CreateShoppingListRequest createShoppingListRequest)
        {
            if (createShoppingListRequest == null || string.IsNullOrEmpty(createShoppingListRequest.Name))
            {
                return new CreateShoppingListResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = "Name parameter cannot be empty"
                };
            }

            var exist = _shoppingListRepository.Get(x => x.Name == createShoppingListRequest.Name);

            if (exist != null && exist.Any()) 
            {
                return new CreateShoppingListResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = "You are trying to add shopping list witch already exist"
                };
            }

            var request = new ShoppingList()
            {
                Name = createShoppingListRequest.Name
            };

            var shoppingList = await _shoppingListRepository.Insert(request);

            var result = _shoppingListMapper.Map(shoppingList);
            await _hubContext.Clients.All.SendAsync(ShoppingListUpdate);

            return new CreateShoppingListResponse()
            {
                ShoppingList = result,
                IsSuccess = true
            };
        }

        public async Task<DeletedShoppingListResponse> DeleteShoppingList(int id)
        {
            var result = await _shoppingListRepository.GetById(id);

            if (result == null)
            {
                return new DeletedShoppingListResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = $"Shopping list was not found"
                };
            }

            await _shoppingListRepository.Delete(id);
            await _hubContext.Clients.All.SendAsync(ShoppingListUpdate);

            return new DeletedShoppingListResponse()
            {
                IsSuccess = true,
                ShoppingListId = id
            };
        }

        public GetAllShoppingListResponse GetAllShoppingList()
        {
            var shoppingList = _shoppingListRepository
                .Get(includeProperties: "ProductList")
                .ToList();

            var result = _shoppingListMapper.Map(shoppingList);

            return new GetAllShoppingListResponse()
            {
                IsSuccess = true,
                ShoppingLists = result
            };
        }
    }
}