using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class AnalyticDefault
    {
        [Key]
        public int AnalyticDefaultId { get; set; }
        [Required]
        public int AnalyticCategory { get; set; }
        [Required]
        public string AnalyticDefaultName { get; set; }
    }
}
