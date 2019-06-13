using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class DefaultDataScore
    {
        [Key]
        public int DefaultDateScoreId { get; set; }
        [Required]
        public int DataHasDefaultId { get; set; }
        [Required]
        public int Score { get; set; }
    }
}
