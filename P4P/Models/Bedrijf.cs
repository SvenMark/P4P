using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace P4P.Models
{
    public class Bedrijf
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Naam { get; set; }

        [Required]
        [StringLength(255)]
        public string Adres { get; set; }

        [Required]
        [StringLength(255)]
        public string Postcode { get; set; }

        [Required]
        [StringLength(255)]
        public string Plaats { get; set; }

        [Required]
        [StringLength(255)]
        [RegularExpression("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Het nummer is niet geldig")]
        public string Telefoonnummer { get; set; }
    }
}