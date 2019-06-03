using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class AnalyticInputDefault
    {
        [Key]
        public int AnalyticInputDefaultId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int AnalyticInputDefaultName { get; set; }
    }
}
