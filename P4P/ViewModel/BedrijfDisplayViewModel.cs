using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using P4P.Models;

namespace P4P.ViewModel
{
    public class BedrijfDisplayViewModel
    {
        public Bedrijf Bedrijf { get; set; }
        public IEnumerable<Gebruiker> Gebruikers { get; set; }
    }
}