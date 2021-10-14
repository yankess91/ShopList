using System.Collections.Generic;

namespace ShopList.Infrastructure.DTOs
{
    public class ShoppingListDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }
    }
}