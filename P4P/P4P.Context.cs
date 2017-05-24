using System.Data.Entity;
using P4P.Models;
namespace P4P
{
    public class P4PContext : DbContext
    {
        public DbSet<Contact> Contacten { get; set; }
        public DbSet<Hoofdcategorie> Hoofdcategories { get; set; }
        public DbSet<Subcategorie> Subcategories { get; set; }
    }
}