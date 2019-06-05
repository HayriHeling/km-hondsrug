using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeLineHasTimeBlock
    {
        [Key]
        public int TimeLineHasTimeBlockId { get; set; }
        [Required]
        public int TimeLineId { get; set; }
        [Required]
        public int TimeBlockId { get; set; }
    }
}
