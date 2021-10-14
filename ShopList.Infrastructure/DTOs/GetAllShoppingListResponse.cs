using System.Collections.Generic;

namespace ShopList.Infrastructure.DTOs
{
    public class GetAllShoppingListResponse : BaseResponse
    {
        public IEnumerable<ShoppingListDto> ShoppingLists { get; set; }
    }
}