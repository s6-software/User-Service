using Microsoft.AspNetCore.Mvc;
using User_Service.Models;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        public UserController(UserContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpPost]
        public async Task<string> RegisterUser(UserRegisterDTO userDTO)
        {
            return $"successfully registered {userDTO.Email}";
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            return Ok(userDTO);
        }

    }
}
