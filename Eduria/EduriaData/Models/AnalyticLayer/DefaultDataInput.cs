using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class DefaultDataInput
    {
        [Key]
        public int DefaultDataInputId { get; set; }
        [Required]
        public int DataHasDefaultId { get; set; }
        [Required, StringLength(int.MaxValue)]
        public string Text { get; set; }
    }
}
