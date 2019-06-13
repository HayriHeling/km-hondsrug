using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
