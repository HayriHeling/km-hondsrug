using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(500, ErrorMessage = "Vul een vraag in.")]
        public string Text { get; set; }
        [MaxLength(500)]
        public string MediaLink { get; set; }
        public Category Category { get; set; }
    }
}
