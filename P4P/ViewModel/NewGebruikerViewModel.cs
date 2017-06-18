using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using P4P.Models;

namespace P4P.ViewModel
{
    public class NewGebruikerViewModel
    {
        public IEnumerable<Bedrijf> Bedrijven { get; set; }
        public Gebruiker Gebruiker { get; set; }
    }
}