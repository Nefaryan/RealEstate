using RealEstate.Models.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace RealEstate.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public Roles Ruolo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        public static implicit operator User(Task<User> v)
        {
            throw new NotImplementedException();
        }
    }
}
