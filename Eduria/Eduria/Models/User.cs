using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class User
    {        
        public int UserId { get; set; }
        [Display(Name = "Voornaam")]
        [DataType(DataType.Text)]
        [MaxLength(45, ErrorMessage="Naam kan niet langer zijn dan 45 karakters.")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public string FirstName { get; set; }
        [Display(Name = "Achternaam")]
        [DataType(DataType.Text)]
        [StringLength(45, ErrorMessage = "Achternaam kan niet langer zijn dan 45 karakters.")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Voer een geldig email adres in.")]
        [StringLength(128)]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public string Email { get; set; }
        [Display(Name = "Identificatie code")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public int UserNum { get; set; }
        [Display(Name = "Klas code")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public int ClassId { get; set; }
        [Display(Name = "Wachtwoord")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minstens 8 karakters bevatten.")]
        [MaxLength(20, ErrorMessage = "Wachtwoord kan maximaal 20 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public string Password { get; set; }
        [Display(Name = "Herhaal Wachtwoord")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minstens 8 karakters bevatten.")]
        [MaxLength(20, ErrorMessage = "Wachtwoord kan maximaal 20 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }

    }
}
