using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class DataHasSubject
    {
        [Key]
        public int DataHasSubjectId { get; set; }
        [Required]
        public int AnalyticDataId { get; set; }
        [Required]
        public int AnalyticSubjectId { get; set; }
    }
}
