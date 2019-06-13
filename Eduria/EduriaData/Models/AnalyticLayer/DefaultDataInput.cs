using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class DefaultDataInput
    {
        [Key]
        public int DefaultDataInputId { get; set; }
        [Required]
        public int DataHasDefaultId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
