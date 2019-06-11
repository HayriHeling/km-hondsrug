using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeLineHasUser
    {
        [Key]
        public int TimeLineHasUserId { get; set; }
        [Required]
        public int TimeLineId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
