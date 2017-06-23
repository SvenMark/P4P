using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using P4P.Models;

namespace P4P.ViewModel
{
    public class NewWinkelmand
    {
        public Product product { get; set; }
        public Winkelwagen winkelwagen { get; set; }
    }
}