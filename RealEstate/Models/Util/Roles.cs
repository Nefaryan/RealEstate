using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models.Util
{
    public class Roles
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
