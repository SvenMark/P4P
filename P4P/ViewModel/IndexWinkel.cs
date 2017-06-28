using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using P4P.Models;

namespace P4P.ViewModel
{
    public class IndexWinkel
    {
        public Gebruiker gebruiker { get; set; }
        public IEnumerable<Hoofdcategorie> hoofdcategorie { get; set; }
        public IEnumerable<Melding> meldingen { get; set; }
        public IEnumerable<Product> aanbiedingen { get; set; }
    }
}