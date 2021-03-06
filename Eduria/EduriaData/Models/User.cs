﻿using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public int MediaSourceId { get; set; }
        [Required, MaxLength(45)]
        public string Firstname { get; set; }
        [Required, MaxLength(45)]
        public string Lastname { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Gebruikersnummer")]
        public int UserNum { get; set; }
        [Required]
        public int UserType { get; set; }
        [Required]
        public int ClassId { get; set; }
        [Required, MaxLength(200)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
        [MaxLength(200)]

        public string Token { get; set; }
    }
}
