﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public string Category { get; set; }
    }
}
