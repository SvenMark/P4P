﻿using System.Data.Entity;
using P4P.Models;
namespace P4P
{
    public class P4PContext : DbContext
    {
        public DbSet<Contact> Contacten { get; set; }
        public DbSet<Hoofdcategorie> Hoofdcategories { get; set; }
        public DbSet<Subcategorie> Subcategories { get; set; }
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Favorietenlijst> Favorietenlijsts { get; set; }
        public DbSet<Melding> Meldingen { get; set; }
        public DbSet<Bedrijf> Bedrijven { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<BestellingProduct> BestellingProducts { get; set; }
        public DbSet<Winkelwagen> Winkelwagens { get; set; }
    }
}