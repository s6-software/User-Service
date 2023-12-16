using System.Text.RegularExpressions;
using User_Service.Models;

namespace User_Service.Services
{
    public interface IUserService
    {
        bool ValidateRegistration(UserRegisterDTO registerDTO);
        bool RegisterUser(UserRegisterDTO registerDTO, string uid);
    }

    public class UserService : IUserService
    {
        private readonly UserContext _userContext;

        public UserService(UserContext context)
        {
            _userContext = context;
        }

        public bool RegisterUser(UserRegisterDTO registerDTO, string uid)
        {
            var newUser = new User
            {
                Name = registerDTO.Name,
                Email = registerDTO.Email,
            };
            _userContext.Users.Add(newUser);
            _userContext.SaveChanges();
            return true;
        }


        public bool ValidateRegistration(UserRegisterDTO user)
        {
            string email = user.Email;
            string password = user.Password;
            string username = user.Name;

            if (IsStrongPassword(password) == false || IsValidEmail(email) == false || IsValidUsername(username) == false)
            {
                return false;
            }

            return true;

        }

        private bool IsStrongPassword(string password)
        {
            string numberPattern = @"\d";
            string capitalLetterPattern = "[A-Z]";
            string specialCharacterPattern = @"[^A-Za-z0-9]";

            bool containsNumber = Regex.IsMatch(password, numberPattern);
            bool containsCapitalLetter = Regex.IsMatch(password, capitalLetterPattern);
            bool containsSpecialChar = Regex.IsMatch(password, specialCharacterPattern);
            bool meetsMinimumLength = password.Length >= 9;

            return containsNumber && containsCapitalLetter && containsSpecialChar && meetsMinimumLength;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, emailPattern);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            string usernamePattern = "^[a-zA-Z]{2,}$";

            return Regex.IsMatch(username, usernamePattern);
        }
    }
}
