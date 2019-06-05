using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeBlockInformation
    {
        [Key]
        public int TimeBlockInformationId { get; set; }
        [Required]
        public int TimeBlockId { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        [MaxLength(256)]
        public string AudioSource { get; set; }
        [MaxLength(256)]
        public string VideoSource { get; set; }
    }
}
