using System.ComponentModel.DataAnnotations;

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
