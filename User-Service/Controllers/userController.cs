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

            try
            {
                string Uid = await _authentication.RegisterAsync(userDTO);
                _userService.RegisterUser(userDTO, Uid);

                return $"successfully registered {userDTO.Email}";
            }
            catch (Exception ex)
            {
                return $"error registering user: {ex.Message}";
            }


        }

        [HttpPost("login")]
        public async Task<LoggedUser> Login(UserLoginDTO userDTO)
        {

            try
            {
                LoggedUser Logged = await _jwtProvider.Login(userDTO);
                return Logged;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser(string token)
        {
            try
            {
                // Extract the JWT token from the Authorization header
                //var token = Request.Headers["Authorization"].ToString().Split(' ')[1];
                var result = await _authentication.UnregisterAsync(token);

                // Return a success message
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Return an error message
                return BadRequest($"Error deleting user: {ex.Message}");
            }
        }
    }
}
