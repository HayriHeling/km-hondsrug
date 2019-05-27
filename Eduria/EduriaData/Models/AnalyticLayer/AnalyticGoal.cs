using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class AnalyticGoal
    {
        [Key]
        public int AnalyticGoalId { get; set; }
        public int AnalyticDataId { get; set; }
        [Required, MaxLength(200)]
        public string AnalyticGoalName { get; set; }
        public int AnalyticGoalScore { get; set; }
    }
}
