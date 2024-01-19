using RealEstate.Models;
using System;

namespace RealEstate.Repository.DTO
{
    public class PropertyDTO
    {
        public string Address {  get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public double MQ { get; set; }
        public int NRooms { get; set; }
        public string Description { get; set; }
        public bool Availability { get; set; }
        public string AvailabilityDate { get; set; }
        public string Owner { get; set; }

        internal PropertyDTO(House h)
        {
            Address = h.Indirizzo;
            City = h.Citta;
            Type = h.Type;
            MQ = h.MQ;
            NRooms = h.NumeroStanze;
            Availability = h.Disponibilita;
            AvailabilityDate = h.DataDisponibilità.ToString();
            Owner = h.Proprietario.Username;
        }
    }
}
