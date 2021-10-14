using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopList.Infrastructure.DTOs;
using ShopList.Infrastructure.Services;
using System.Threading.Tasks;

namespace ShopList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _shoppingListService.GetAllShoppingList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateShoppingListRequest shoppingList)
        {
            var result = await _shoppingListService.CreateShoppingList(shoppingList);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int request)
        {
            var result = await _shoppingListService.DeleteShoppingList(request);

            return Ok(result);
        }
    }
}