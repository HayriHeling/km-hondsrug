using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required, MaxLength(200)]
        public string Text { get; set; }
        [Required, MaxLength(500)]
        public string MediaLink { get; set; }
        [Required]
        public int MediaType { get; set; }
    }
}
