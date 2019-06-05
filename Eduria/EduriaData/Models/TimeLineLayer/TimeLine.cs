using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeLine
    {
        [Key]
        public int TimeLineId { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
