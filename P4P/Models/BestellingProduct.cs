using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace P4P.Models
{
    public class BestellingProduct
    {
        [Key, Column(Order = 0), ForeignKey("Bestelling")]
        public int Bestelling_Id { get; set; }
        [Key, Column(Order = 1), ForeignKey("Product")]
        public int Product_Id { get; set; }

        public Bestelling Bestelling { get; set; }
        public Product Product { get; set; }

        public int Aantal { get; set; }
    }
}