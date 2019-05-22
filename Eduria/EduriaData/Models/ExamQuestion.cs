using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class ExamQuestion
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}
