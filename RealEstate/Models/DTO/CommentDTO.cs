using RealEstate.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace RealEstate.Repository.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }

        internal CommentDTO(Comments c) { 
            Id = c.Id;
            Text = c.Testo;
            User = c.Utente.Username;
            Date = c.Date;
        }   
    }
}
