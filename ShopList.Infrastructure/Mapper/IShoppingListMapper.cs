using ShopList.Infrastructure.DTOs;
using ShopList.Infrastructure.Model;
using System.Collections.Generic;

namespace ShopList.Infrastructure.Mapper
{
    public interface IShoppingListMapper
    {
        IEnumerable<ShoppingListDto> Map(IEnumerable<ShoppingList> shoppingList);

        ShoppingListDto Map(ShoppingList shoppingList);

        ShoppingList Map(ShoppingListDto shoppingListDto);
    }
}