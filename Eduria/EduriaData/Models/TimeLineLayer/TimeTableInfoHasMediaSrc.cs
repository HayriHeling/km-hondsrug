using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeTableInfoHasMediaSrc
    {
        [Key]
        public int TimeTableInfoHasMediaSrcId { get; set; }
        [Required]
        public int TimeTableInformationId { get; set; }
        [Required]
        public int MediaSourceId { get; set; }
    }
}
