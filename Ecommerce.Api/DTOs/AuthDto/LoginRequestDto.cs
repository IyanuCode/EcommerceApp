
namespace Ecommerce.Api.DTOs.AuthDto
{
    public class LoginRequestDto
    {
        public string Username { get; set; } = string.Empty; //Username sent by client
        public string Password { get; set; } = string.Empty; //Password sent by client
    }
}