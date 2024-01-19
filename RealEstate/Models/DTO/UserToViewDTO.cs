using RealEstate.Models;

namespace RealEstate.Repository.DTO
{
    public class UserToViewDTO : IUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role{  get; set; }
 

        internal UserToViewDTO(User u) {
            Username = u.Username;
            Email = u.Email;
            Role = u.Ruolo.Name;
        }
    }
}
