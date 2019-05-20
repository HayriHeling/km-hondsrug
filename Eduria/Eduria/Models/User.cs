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
        [Display(Name = "Naam")]
        [DataType(DataType.Text)]
        [StringLength(30, ErrorMessage="De Naam kan niet langer zijn dan 30 karakters.")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public string Name { get; set; }
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
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Wachtwoord moet minstens 8 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public string Password { get; set; }

    }
}
