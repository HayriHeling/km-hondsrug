using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class User : IdentityUser
    {
        [Required, MaxLength(45, ErrorMessage = "Vul een voornaam in.")]
        public string Firstname { get; set; }
        [MaxLength(10)]
        public string Infix { get; set; }
        [Required, MaxLength(45, ErrorMessage = "Vul een achternaam in.")]
        public string Lastname { get; set; }
        [Required, MaxLength(100, ErrorMessage = "Vul een e-mailadres in.")]
        public override string Email { get; set; }
        public int StudNum { get; set; }
        public int UserType { get; set; }
        public int ClassId { get; set; }
        [Required(ErrorMessage = "Vul een wachtwoord in."), MaxLength(20, ErrorMessage = "Het wachtwoord mag maximaal 20 karakters lang zijn.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Username")]
        [Required(ErrorMessage = "Vul een username in.")]
        public override string UserName { get ; set; }
    }
}
