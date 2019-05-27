using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class AnalyticSubject
    {
        [Key]
        public int AnalyticSubjectId { get; set; }
        public int AnalyticDataId { get; set; }
        [Required, MaxLength(200)]
        public string AnalyticSubjectName { get; set; }
        public int AnalyticSubjectScore { get; set; }
    }
}
