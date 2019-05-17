using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public int QuestionId { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Vul een antwoord in.")]
        public string Text { get; set; }
        public int Correct { get; set; }
    }
}
