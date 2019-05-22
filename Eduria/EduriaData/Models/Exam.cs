using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        public Category Category { get; set; }
    }
}
