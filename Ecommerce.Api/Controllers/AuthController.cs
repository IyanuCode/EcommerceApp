using AutoMapper;
using Ecommerce.Api.DTOs.AuthDto;
using Ecommerce.Data.Interfaces.IAuthServices;
using Microsoft.AspNetCore.Mvc;
namespace Ecommerce.Api.Controllers
{
    [ApiController] //This attribute makes the controller behave as a web API controller
    [Route("api/[controller]")] //This attribute defines the routing pattern for the controller
    public class AuthController : ControllerBase
    {
        private readonly IAuthServiceRepository _authServiceRepository; // Repository for authentication services
        private readonly IMapper _mapper; // Mapper for DTO to Entity conversion
        public readonly IConfiguration _config; // to read Jwt settings from appsettings.json

        public AuthController(IAuthServiceRepository authServiceRepository, IMapper mapper, IConfiguration config)
        {
            _authServiceRepository = authServiceRepository; // repository handles DB access
            _mapper = mapper;         // mapper handles DTO to Entity
            _config = config;         // config handles app settings
        }
        /*---------------------------------Register New User---------------------------------*/
        [HttpPost("register")] // This attribute says: this endpoint responds to HTTP POST requests at "api/Auth/register".
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            //1. Map DTO to User entity
            var user = _mapper.Map<Data.Models.Auth.User>(registerRequest);
            //2. Create user in DB
            var createdUser = await _authServiceRepository.CreateUserAsync(user, registerRequest.Password);
            if (createdUser == null)
            {
                return BadRequest("User registration failed. Username may already be taken.");
            }
            return Ok(new { Message = "User registered successfully", UserId = createdUser.Id });
        }

        /*-----------------------------------Login User--------------------------------------*/
        [HttpPost("login")] // This attribute says: this endpoint responds to HTTP POST requests at "api/Auth/login".
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            //1. Find the user from DB
            var user = await _authServiceRepository.FindUserAsync(loginRequest.Username, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            //2. Generate JWT token
            var accessToken = _authServiceRepository.GenerateAccessToken(user);

            //3. Generate Refresh Token
            var refreshToken = _authServiceRepository.GenerateRefreshToken();

            //4. Save Refresh Token to DB
            await _authServiceRepository.SaveRefreshTokenAsync(user.Id, refreshToken);

            //5. Return tokens to client
            var response = new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(response);
        }
    }
}
