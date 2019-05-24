using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required, MaxLength(45)]
        public string Name { get; set; }
        [Required, MaxLength(256)]
        public string Description { get; set; }
    }
}
