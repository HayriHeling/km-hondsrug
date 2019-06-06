using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduriaData.Models
{
    public class MediaSource
    {
        [Key]
        public int MediaSourceId { get; set; }
        [Required, MaxLength(256)]
        public string Source { get; set; }
        [Required]
        public int MediaType { get; set; }
    }
}
