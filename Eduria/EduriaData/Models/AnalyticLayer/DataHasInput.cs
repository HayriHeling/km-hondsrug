using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class DataHasInput
    {
        [Key]
        public int DataHasInputId { get; set; }
        [Required]
        public int AnalyticDataId { get; set; }
        [Required]
        public int AnalyticInputDefaultId { get; set; }
        public int AnalyticInputDefaultText { get; set; }
    }
}
