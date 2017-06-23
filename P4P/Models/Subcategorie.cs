using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace P4P.Models
{
    public class Subcategorie
    {
        public int Id { get; set; }

        [Required]
        public Hoofdcategorie Hoofdcategorie { get; set; }

        [Required]
        [StringLength(255)]
        public string Naam { get; set; }
    }
}