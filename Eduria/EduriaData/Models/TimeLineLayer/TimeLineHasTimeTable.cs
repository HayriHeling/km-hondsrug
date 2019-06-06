using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeLineHasTimeTable
    {
        [Key]
        public int TimeLineHasTimeTableId { get; set; }
        [Required]
        public int TimeLineId { get; set; }
        [Required]
        public int TimeTableId { get; set; }
    }
}
