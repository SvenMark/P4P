using System.Data.Entity;
using P4P.Models;
namespace P4P
{
    public class P4PContext : DbContext
    {
        public DbSet<Contact> Contacten { get; set; }
    }
}