using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
