using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using P4P.Models;

namespace P4P.ViewModel
{
    public class NewArtikelViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Hoofdcategorie> Hoofdcategorie { get; set; }
        public IEnumerable<Subcategorie> Subcategorie { get; set; }
    }
}