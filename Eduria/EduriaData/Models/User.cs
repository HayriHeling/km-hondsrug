using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required, MaxLength(45)]
        public string Firstname { get; set; }
        [Required, MaxLength(45)]
        public string Lastname { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public int StudNum { get; set; }
        [Required]
        public int UserType { get; set; }
        [Required]
        public int ClassId { get; set; }
        [Required, MaxLength(20)]
        public string Password { get; set; }
    }
}
