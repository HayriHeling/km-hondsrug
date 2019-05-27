using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class DataHasMethod
    {
        [Key]
        public int DataHasMethodId { get; set; }
        [Required]
        public int AnalyticDataId { get; set; }
        [Required]
        public int AnalyticMethodId { get; set; }
    }
}
