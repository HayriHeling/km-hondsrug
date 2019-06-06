using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class AnalyticData
    {
        [Key]
        public int AnalyticDataId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PeriodId { get; set; }
        [Required]
        public string ExamCode { get; set; }
    }
}
