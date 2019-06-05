using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeBlockHasUser
    {
        [Key]
        public int TimeBlockHasUserId { get; set; }
        [Required]
        public int TimeBlockId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
