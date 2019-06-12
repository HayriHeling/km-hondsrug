using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduriaData.Models
{
    public class Config
    {
        [Key]
        public int ConfigId { get; set; }
        [Required, MaxLength(256)]
        public string ToMail { get; set; }
        [Required, MaxLength(256)]
        public string UserMail { get; set; }
        [Required, MaxLength(256)]
        public string Password { get; set; }
        [Required, MaxLength(256)]
        public string FromMail { get; set; }
        [Required]
        public int SMTPPort { get; set; }
        [Required, MaxLength(256)]
        public string Host { get; set; }
        [Required]
        public DateTime EntryCreatedAt { get; set; }
    }
}
