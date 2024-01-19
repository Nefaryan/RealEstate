using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Models
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }
        public string Testo { get; set; }
        [ForeignKey("UserId")]
        public User Utente { get; set; }
        public DateTime Date { get; set; }
    }
}
