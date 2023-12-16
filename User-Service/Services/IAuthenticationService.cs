using FirebaseAdmin.Auth;
using System.Net.Http;
using System.Text.Json.Serialization;
using User_Service.Models;

namespace User_Service.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(UserRegisterDTO userDTO);
    }

    public class AuthenticationService : IAuthenticationService
    {
        public async Task<string> RegisterAsync(UserRegisterDTO userDTO)
        {
            var userArgs = new UserRecordArgs
            {
                Email = userDTO.Email,
                Password = userDTO.Password,
            };

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            return userRecord.Uid;
        }
    }
}
