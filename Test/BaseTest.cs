using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User_Service.Models;
using User_Service.Services;

namespace Test
{
    public class BaseTest
    {
        protected IUserService _userService;
        protected UserRegisterDTO _registerDTO;

        public BaseTest()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase("Testing")
                .Options;

            using var context = new UserContext(options);
            _userService = new UserService(context);
        }

        protected void SetupRegisterDTO()
        {
            _registerDTO = new UserRegisterDTO
            {
                Name = "JohnDoe",
                Email = "JohnDoe@hotmail.com",
                Password = "SuperSecure1!",
            };
        }
    }
}
