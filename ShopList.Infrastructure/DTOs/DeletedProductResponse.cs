using System.Collections.Generic;

namespace ShopList.Infrastructure.DTOs
{
    public class DeletedProductResponse : BaseResponse
    {
        public IEnumerable<int> ProductIds { get; set; }
    }
}