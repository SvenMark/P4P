using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using P4P.Models;

namespace P4P.ViewModel
{
    public class NewOrderDetails
    {
        public Bedrijf Bedrijf { get; set; }
        public Gebruiker Gebruiker { get; set; }
        public IEnumerable<BestellingProduct> BestellingProducts { get; set; }
        public Bestelling Bestelling { get; set; }
    }
}