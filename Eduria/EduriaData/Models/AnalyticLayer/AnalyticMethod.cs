using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class AnalyticMethod
    {
        [Key]
        public int AnalyticMethodId { get; set; }
        [Required, MaxLength(200)]
        public string AnalyticMethodName { get; set; }
        [Required]
        public int AnalyticMethodScore { get; set; }
    }
}
