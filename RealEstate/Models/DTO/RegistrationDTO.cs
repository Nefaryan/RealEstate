using RealEstate.Models;

namespace RealEstate.Repository.DTO
{
    public class RegistrationDTO : IUserDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public string? email { get; set; }  
        public RegistrationDTO() { }
        internal RegistrationDTO(User u)
        {
            Username = u.Username;
            Password = u.Salt;
            email = u.Email;
        }
    }
}
