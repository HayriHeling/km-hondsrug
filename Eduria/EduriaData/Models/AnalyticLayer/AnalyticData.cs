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
        public int Period { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string ExamCode { get; set; }
        public string Reflection { get; set; }
        public string UniqueMethodName { get; set; }
        public int UniqueMethodScore { get; set; }
    }
}
