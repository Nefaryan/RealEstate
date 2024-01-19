using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace RealEstate.Models
{
    public class House
    {
        [Key]
        public int Id { get; set; }
        public string Indirizzo { get; set; }   
        public string Citta { get; set; }   
        public string Type { get; set; }
        public double MQ { get; set; }
        public int NumeroStanze { get; set; }
        public string Descrizione {  get; set; }
        public bool Disponibilita { get; set; }
        public DateTime DataDisponibilità { get; set; }
        public ICollection<Issue> Issue { get; set; }
        [ForeignKey("OwnerId")]
        public User Proprietario { get; set; }
        [ForeignKey("TenantId")]
        public User Inquilino { get; set; }

        public static implicit operator House(Task<House> v)
        {
            throw new NotImplementedException();
        }
    }
}
