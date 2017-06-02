using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace P4P.Models
{
    public class Gebruiker
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Voornaam { get; set; }

        [StringLength(255)]
        public string Tussenvoegsel { get; set; }

        [Required]
        [StringLength(255)]
        public string Achternaam { get; set; }

        [Required]
        [StringLength(255)]
        [RegularExpression("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Het nummer is niet geldig")]
        public string Telefoonnummer { get; set; }

        [Required]
        [StringLength(255)]
        public string Emailadres { get; set; }

        [Required]
        [StringLength(255)]
        public string Bedrijfsnaam { get; set; }

        [Required]
        [StringLength(255)]
        public string Adres { get; set; }

        [Required]
        [StringLength(255)]
        public string Postcode { get; set; }

        [Required]
        [StringLength(255)]
        public string Woonplaats { get; set; }

        [StringLength(255)]
        [MinLength(8)]
        public string Wachtwoord { get; set; }

        [StringLength(255)]
        public string LoginToken { get; set; }

        [StringLength(255)]
        public string Token { get; set; }
    }
}