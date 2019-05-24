using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class DataHasGoal
    {
        [Key]
        public int DataHasGoalId { get; set; }
        [Required]
        public int AnalyticDataId { get; set; }
        [Required]
        public int AnalyticGoalId { get; set; }
    }
}
