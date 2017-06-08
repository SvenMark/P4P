using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace P4P.Models
{
    public class Product
    {
        public int Id { get; set; }

        public Hoofdcategorie Hoofdcategorie { get; set; }
        public Subcategorie Subcategorie { get; set; }

        public virtual ICollection<Favorietenlijst> Favorietenlijsts { get; set; }

        public virtual ICollection<Bestelling> Bestellingen { get; set; }

        [Required]
        [StringLength(255)]
        public string Naam { get; set; }

        [Required]
        public double Prijs { get; set; }

        [Required]
        [StringLength(255)]
        public string Verkoopeenheid { get; set; }

        [Required]
        public string Beschrijving { get; set; }

        [Required]
        [StringLength(255)]
        public string Code { get; set; }

        [Required]
        public string Specificaties { get; set; }
    }
}