using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace P4P.Models
{
    public class Winkelwagen
    {
        [Key, ForeignKey("Product")]
        public int Product_Id { get; set; }

        public Product Product { get; set; }

        public int Aantal { get; set; }
        
    }
}