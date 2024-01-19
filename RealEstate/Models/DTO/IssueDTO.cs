using RealEstate.Models;
using System;


namespace RealEstate.Repository.DTO
{
    public class IssueDTO
    {
        public string? Name { set; get; }
        public string? Description { set; get; }
        public string? State {  set; get; }
        public DateTime DataApertura { set; get; }
        public DateTime DataChiusura { set; get; }
        public House House { set; get; }
        public User User { set; get; }

        
        public IssueDTO(Issue I)
        {
            Name = I.Name;
            Description = I.Description;
            State = I.Stato;
            DataApertura = I.DataApertura;
            DataChiusura = I.DataChiusura;
            House = I.Proprietà;
            User = I.Utente;
        }

        public IssueDTO() { } 
    }
}
