using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class updatePasswordModel
    {
        [Display(Name = "Oud wachtwoord")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minstens 8 karakters bevatten.")]
        [MaxLength(20, ErrorMessage = "Wachtwoord kan maximaal 20 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public string oldPassword { get; set; }
        [Display(Name = "Nieuw wachtwoord")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minstens 8 karakters bevatten.")]
        [MaxLength(20, ErrorMessage = "Wachtwoord kan maximaal 20 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public string newPassword { get; set; }
        [Display(Name = "Herhaal Wachtwoord")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minstens 8 karakters bevatten.")]
        [MaxLength(20, ErrorMessage = "Wachtwoord kan maximaal 20 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }

    }

}
