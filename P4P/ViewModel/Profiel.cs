using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using P4P.Models;

namespace P4P.ViewModel
{
    public class Profiel
    {
        public Gebruiker gebruiker { get; set; }
        public IEnumerable<Contact> contacts { get; set; }
    }
}