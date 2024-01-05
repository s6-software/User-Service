using System.ComponentModel.DataAnnotations;

namespace User_Service.Models
{
    public class User
    {
        [Key]
        [Required]
        public required string UID { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
    public class UserLoginDTO
    {
        public required string Password { get; set; }
        public required string Email { get; set; }
    }

    public class LoggedUser
    {
        public string Uid { get; set; }
        public string? Name { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
    }

    public class UserRegisterDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
    }
}
