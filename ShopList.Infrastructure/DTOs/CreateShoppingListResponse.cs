namespace ShopList.Infrastructure.DTOs
{
    public class CreateShoppingListResponse : BaseResponse
    {
        public ShoppingListDto ShoppingList { get; set; }
    }
}