namespace User_Service.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        }
    public class UserDTO
    {
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}
