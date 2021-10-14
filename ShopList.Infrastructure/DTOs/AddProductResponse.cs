using System.Collections.Generic;

namespace ShopList.Infrastructure.DTOs
{
    public class AddProductResponse : BaseResponse
    {
        public IEnumerable<ProductDto> Products { get; set; }
    }
}