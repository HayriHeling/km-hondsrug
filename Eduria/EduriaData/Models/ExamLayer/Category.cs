using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.ExamLayer
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required, MaxLength(45)]
        public string CategoryName { get; set; }
    }
}
