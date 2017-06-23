using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using P4P.Models;

namespace P4P.ViewModel
{
    public class NewSubcategorie
    {
        public SubcategorieProducts Subcategorie { get; set; }
        public Hoofdcategorie Hoofdcategorie { get; set; }
    }
}