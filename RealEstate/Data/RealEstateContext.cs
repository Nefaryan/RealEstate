using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;
using RealEstate.Models.Util;

namespace RealEstate.Data
{
    public class RealEstateContext :DbContext
    {
        public RealEstateContext(DbContextOptions options) : base(options) { }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<User> Utente { get; set; }
        public DbSet<House> Proprietà { get; set; }
        public DbSet<Comments> Commenti { get; set; }
        public DbSet<Issue> Issue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }
    }


}




