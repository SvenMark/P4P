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
        public byte Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Naam { get; set; }
    }
}