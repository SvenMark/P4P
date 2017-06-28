using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using P4P.Models;

namespace P4P.ViewModel
{
    public class SubcategorieProducts
    {
        public IEnumerable<Product> product { get; set; }
        public Hoofdcategorie hoofdcategorie { get; set; }
        public Subcategorie subcategorie { get; set; }
        public IEnumerable<Subcategorie> subcategories { get; set; }
        public Winkelwagen winkelwagen { get; set; }
    }
}