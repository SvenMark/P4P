using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace P4P.Models
{
    public class Bestelling
    {
        public int Id { get; set; }

        [Required]
        public double Prijs { get; set; }

        public Bedrijf Bedrijf { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? Afleverdatum { get; set; }

        [Required]
        public Gebruiker Gebruiker { get; set; }

        [StringLength(255)]
        public string Opmerking { get; set; }

        public bool Afgerond { get; set; }

        public virtual ICollection<BestellingProduct> Producten { get; set; }

        [StringLength(255)]
        public string AfleverAdres { get; set; }

        [StringLength(255)]
        public string AfleverPostcode { get; set; }

        [StringLength(255)]
        public string AfleverPlaats { get; set; }
    }
}