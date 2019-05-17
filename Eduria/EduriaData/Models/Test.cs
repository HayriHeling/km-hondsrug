using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public Category Category { get; set; }
    }
}
