using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(45)]
        public string CategoryName { get; set; }
    }
}
