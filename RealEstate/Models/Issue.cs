using RealEstate.Models.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Models
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("StatesId")]
        public string Stato { get; set; }
        public DateTime DataApertura { get; set; }
        public DateTime DataChiusura { get; set; }
        [ForeignKey("HouseId")]
        public House Proprietà { get; set; }
        [ForeignKey("UserId")]
        public User Utente { get; set; }
        public ICollection<Comments> Commenti { get; set; }
    }
}
