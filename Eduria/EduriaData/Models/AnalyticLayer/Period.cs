using System;
using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.AnalyticLayer
{
    public class Period
    {
        [Key]
        public int PeriodId { get; set; }
        [Required]
        public int PeriodNum { get; set; }
        [Required]
        public DateTime PeriodStart { get; set; }
        [Required]
        public DateTime PeriodEnd { get; set; }
        [Required]
        public int SchoolYearStart { get; set; }
        [Required]
        public int SchoolYearEnd { get; set; }
    }
}
