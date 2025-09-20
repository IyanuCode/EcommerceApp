
namespace Ecommerce.Api.DTOs.AuthDto
{
    public class LoginResponseDto
    {
        
    // This DTO represents the data we send back to the client after login
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}