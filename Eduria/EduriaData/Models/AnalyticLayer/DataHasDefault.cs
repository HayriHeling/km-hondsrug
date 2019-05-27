using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class DataHasDefault
    {
        [Key]
        public int DataHasDefaultId { get; set; }
        [Required]
        public int AnalyticDataId { get; set; }
        [Required]
        public int AnalyticDefaultId { get; set; }
        public int Score { get; set; }
    }
}
