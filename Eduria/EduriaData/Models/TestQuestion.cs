using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class TestQuestion
    {
        [ForeignKey("Id")]
        public int TestId { get; set; }
        [ForeignKey("Id")]
        public int QuestionId { get; set; }
    }
}
