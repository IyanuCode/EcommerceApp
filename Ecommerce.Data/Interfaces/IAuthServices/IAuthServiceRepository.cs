

using Ecommerce.Data.Models.Auth;

namespace Ecommerce.Data.Interfaces.IAuthServices
{
    public interface IAuthServiceRepository
    {
        Task <User?> CreateUserAsync(User user, string password);
        Task<User?> FindUserAsync(string username, string password);
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        Task SaveRefreshTokenAsync(int userId, string refreshToken);


    }
}