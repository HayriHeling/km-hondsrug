using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(45, ErrorMessage = "Vul een voornaam in.")]
        public string Firstname { get; set; }
        [Required, MaxLength(45, ErrorMessage = "Vul een achternaam in.")]
        public string Lastname { get; set; }
        [Required, MaxLength(100, ErrorMessage = "Vul een e-mailadres in.")]
        public string Email { get; set; }
        public int StudNum { get; set; }
        public int UserType { get; set; }
        public int ClassId { get; set; }
        [Required, MaxLength(500, ErrorMessage = "Vul een wachtwoord in.")]
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
