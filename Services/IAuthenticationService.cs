using FirebaseAdmin.Auth;
using System.Net.Http;
using System.Text.Json.Serialization;
using User_Service.Models;

namespace API.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(UserDTO userDTO);
    }

    public class AuthenticationService : IAuthenticationService
    {
        public async Task<string> RegisterAsync(UserDTO userDTO)
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
