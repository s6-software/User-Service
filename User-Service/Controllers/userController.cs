using Microsoft.AspNetCore.Mvc;
using User_Service.Models;
using User_Service.Services;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authentication;
        private readonly IJwtProvider _jwtProvider;

        public UserController(IUserService userContext, IAuthenticationService authentication, IJwtProvider jwtProvider)
        {
            _userService = userContext;
            _authentication = authentication;
            _jwtProvider = jwtProvider;
        }

        [HttpPost("register")]
        public async Task<string> RegisterUser(UserRegisterDTO userDTO)
        {
            if (_userService.ValidateRegistration(userDTO) == false)
            {
                return "invalid input";
            }

            string Uid = await _authentication.RegisterAsync(userDTO);

            _userService.RegisterUser(userDTO, Uid);

            return $"successfully registered {userDTO.Email}";
        }

        [HttpPost("login")]
        public async Task<string> Login(UserLoginDTO userDTO)
        {
            string token = await _jwtProvider.Login(userDTO);

            return token;
        }

    }
}
