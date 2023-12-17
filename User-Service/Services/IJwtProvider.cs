using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json.Serialization;
using User_Service.Models;

namespace User_Service.Services
{
    public interface IJwtProvider
    {
        Task<LoggedUser> Login(UserLoginDTO userDTO);
    }

    public class JwtProvider : IJwtProvider
    {
        private readonly HttpClient _httpClient;
        private readonly UserContext _userContext;

        public JwtProvider(HttpClient httpClient, UserContext userContext)
        {
            _httpClient = httpClient;
            _userContext = userContext;
        }
        public async Task<LoggedUser> Login(UserLoginDTO userDTO)
        {
            var request = new
            {
                userDTO.Email,
                userDTO.Password,
                returnSecureToken = true
            };
            var response = await _httpClient.PostAsJsonAsync("", request);

            var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();

            var user = await _userContext.Users.FirstOrDefaultAsync(x => x.Email == userDTO.Email);

            return new LoggedUser
            {
                Email = userDTO.Email,
                Token = authToken!.IdToken,
                Name = user!.Name
            };
        }

        public class AuthToken
        {
            [JsonPropertyName("kind")]
            public string Kind { get; set; }
            [JsonPropertyName("localId")]
            public string LocalId { get; set; }
            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; }

            [JsonPropertyName("idToken")]
            public string IdToken { get; set; }

            [JsonPropertyName("registered")]
            public bool Registered { get; set; }
            [JsonPropertyName("refreshToken")]
            public string RefreshToken { get; set; }

            [JsonPropertyName("expiresin")]
            public long ExpiresIn { get; set; }


        }

    }
}
