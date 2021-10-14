namespace ShopList.Infrastructure.DTOs
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }
    }
}