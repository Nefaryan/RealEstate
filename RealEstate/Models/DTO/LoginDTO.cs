using RealEstate.Models;

namespace RealEstate.Repository.DTO
{
    public class LoginDTO : IUserDto
    {
        public string? Username { get; set; }
        public string? Password { get ; set ; }

        public LoginDTO() { }
        internal LoginDTO(User u)
        {
            Username = u.Username;
            Password = u.Salt;
        }
    }
}
