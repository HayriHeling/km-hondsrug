﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeTableInformation
    {
        [Key]
        public int TimeTableInformationId { get; set; }
        [Required]
        public int TimeTableId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required, StringLength(int.MaxValue)]
        public string Name { get; set; }
        [StringLength(int.MaxValue)]
        public string Description { get; set; }
        [Required]
        public int BeforeChrist { get; set; }
        [Required]
        public int Year { get; set; }
    }
}
