
namespace Ecommerce.Api.DTOs.EcommerceStore
{
    public class ReadEcommerceStoreDto
    {
        public int Id { get; set; }
        public string? Product { get; set; }
        public string? Category { get; set; }
        public string? Orders { get; set; }
    }
}