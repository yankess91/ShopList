using Microsoft.AspNetCore.Mvc;
using ShopList.Infrastructure.DTOs;
using ShopList.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddProductRequest request)
        {
            var result = await _productService.AddProductToShoppingList(request);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Post(IEnumerable<int> request)
        {
            var result = await _productService.DeleteProductsFromShoppingList(request);

            return Ok(result);
        }
    }
}