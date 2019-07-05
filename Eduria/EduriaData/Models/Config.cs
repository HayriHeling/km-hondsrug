using System;
using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class Config
    {
        [Key]
        public int ConfigId { get; set; }
        [Required, MaxLength(256)]
        public string FromMail { get; set; }
        [Required, MaxLength(256)]
        public string Password { get; set; }
        [Required, MaxLength(256)]
        public string Subject { get; set; }
        [Required, StringLength(int.MaxValue)]
        public string Body { get; set; }
        [Required]
        public int SMTPPort { get; set; }
        [Required, MaxLength(256)]
        public string Host { get; set; }
        [Required]
        public DateTime EntryChangedAt { get; set; }
    }
}
