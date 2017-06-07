using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace P4P.Models
{
    public class Aanbiedingen
    {
        public int Id { get; set; }

        public Product ProductId { get; set; }

        [Required]
        public double Prijs { get; set; }

        [Required]
        [StringLength(255)]
        public string Beschrijving { get; set; }

    }
}