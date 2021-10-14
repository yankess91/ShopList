using ShopList.Infrastructure.DTOs;
using ShopList.Infrastructure.Mapper;
using ShopList.Infrastructure.Model;
using System.Collections.Generic;
using System.Linq;

namespace ShopList.Logic.Mapper
{
    public class ShoppingListMapper : IShoppingListMapper
    {
        public IEnumerable<ShoppingListDto> Map(IEnumerable<ShoppingList> shoppingList)
        {
            return shoppingList.Select(x => Map(x));
        }

        public ShoppingListDto Map(ShoppingList shoppingList)
        {
            return new ShoppingListDto()
            {
                Id = shoppingList.Id,
                Name = shoppingList.Name,
                Products = shoppingList.ProductList?.Select(p => new ProductDto
                {
                    Name = p.Name,
                    Type = p.Type,
                    Price = p.Price,
                    Id = p.Id
                })
            };
        }

        public ShoppingList Map(ShoppingListDto shoppingListDto)
        {
            return new ShoppingList()
            {
                Id = shoppingListDto.Id,
                Name = shoppingListDto.Name,
                ProductList = shoppingListDto.Products != null ? shoppingListDto.Products.Select(x => new Product()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type,
                    Price = x.Price
                })
                .ToList()
                : new List<Product>()
            };
        }
    }
}