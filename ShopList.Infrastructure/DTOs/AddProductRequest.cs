using System.Collections.Generic;

namespace ShopList.Infrastructure.DTOs
{
    public class AddProductRequest
    {
        public IEnumerable<ProductDto> Products { get; set; }

        public int ShoppingListId { get; set; }
    }
}