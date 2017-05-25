using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace P4P.Models
{
    public class Favorietenlijst
    {
        public int Id { get; set; }

        public Gebruiker Gebruiker { get; set; }

        public virtual ICollection<Product> Producten { get; set; }

        public string Naam { get; set; }
    }
}