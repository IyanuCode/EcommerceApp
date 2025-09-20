

using Ecommerce.Data.Data;
using Ecommerce.Data.Interfaces.IAuthServices;
using Ecommerce.Data.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Data.Repositories.AuthService
{
    public class AuthServiceRepository : IAuthServiceRepository
    {
        private readonly EcommerceStoreDbContext _dbContext; // Database context for accessing the database
        private readonly IConfiguration _config; // Configuration for accessing app settings
        private readonly PasswordHasher<User> _hasher; // Password hasher for verifying passwords
        public AuthServiceRepository(EcommerceStoreDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext; // Initialize the database context
            _config = config;   // Initialize the configuration
            _hasher = new PasswordHasher<User>(); // Initialize the password hasher
        }
        /*---------------------------------Find User in DB---------------------------------*/
        public async Task<User?> FindUserAsync(string username, string enteredPassword)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username);
            if (user == null || !VerifyPassword(enteredPassword, user.PasswordHash))
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        /*---------------------------------Generate Access Token---------------------------------*/
        public string GenerateAccessToken(User user)
        {
            var key = _config["JwtSettings:SecretKey"];
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];

            //Claims: User information to be included in the token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Unique identifier for the user
                new Claim(ClaimTypes.Name, user.Username),// Username of the user
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),// Subject of the token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),// Unique identifier for the token
            // if i want to assign roles to users, this is where i will add them    
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)); // Symmetric security key for signing the token
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // Signing credentials using HMAC SHA256 algorithm
            //it is important to sign the token to ensure its integrity and authenticity and to be sure that it has not been tampered with while in transit

            // Am creating and generating the token here
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Set the claims for the token
                Expires = DateTime.UtcNow.AddMinutes(30), // Token expiration time (30 minutes)
                Issuer = issuer, // Issuer of the token
                Audience = audience, // Audience of the token
                SigningCredentials = credentials // Signing credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler(); // Token handler for creating the token
            var token = tokenHandler.CreateToken(tokenDescriptor); // Create the token
            return tokenHandler.WriteToken(token); // Return the serialized token
        }

        /*---------------------------------Generate Refresh Token---------------------------------*/
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /*---------------------------------Save Refresh Token to DB---------------------------------*/
        public async Task SaveRefreshTokenAsync(int userId, string refreshToken)
        {
            var token = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _dbContext.RefreshTokens.Add(token);
            await _dbContext.SaveChangesAsync();
        }

        /*---------------------------------Password helper---------------------------------*/
        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var result = _hasher.VerifyHashedPassword(null, storedHash, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }

        /*---------------------------------Create User---------------------------------*/
        public async Task<User?> CreateUserAsync(User user, string password)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Username.ToLower() == user.Username.ToLower()))
            {
                return null; // Username already exists
            }
            
            user.PasswordHash = _hasher.HashPassword(null, password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }



    }
}