using Azure.Messaging.ServiceBus;
using FirebaseAdmin.Auth;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using User_Service.Models;

namespace User_Service.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(UserRegisterDTO userDTO);

        Task<string> UnregisterAsync(string token);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        public AuthenticationService(IUserService userService)
        {
            _userService = userService;
        }
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

        public async Task<string> UnregisterAsync(string token)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                string userId = decodedToken.Uid;

                bool result = _userService.DeleteUser(userId);
                if (result == false)
                {
                    return "user not found";
                }
                await FirebaseAuth.DefaultInstance.DeleteUserAsync(userId);

                string jsonPath = Environment.GetEnvironmentVariable("MESSAGE_BUS");

                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(jsonPath)
                    .Build();

                string connectionString = config["connection"];

                var client = new ServiceBusClient(connectionString);

                var sender = client.CreateSender("cascadinguser");

                var jsonObject = new { id = userId };
                var jsonMessage = JsonSerializer.Serialize(jsonObject);
                var message = new ServiceBusMessage(jsonMessage);

                await sender.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "user successfully deleted :)";


        }
    }
}
