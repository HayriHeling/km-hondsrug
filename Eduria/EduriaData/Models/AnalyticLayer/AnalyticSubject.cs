using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class AnalyticSubject
    {
        [Key]
        public int AnalyticSubjectId { get; set; }
        [Required, MaxLength(200)]
        public string AnalyticSubjectName { get; set; }
        [Required]
        public int AnalyticSubjectScore { get; set; }
    }
}
