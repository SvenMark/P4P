using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace P4P.Models
{
    public class Bedrijf
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Bedrijfsnaam")]
        public string Naam { get; set; }
    }
}