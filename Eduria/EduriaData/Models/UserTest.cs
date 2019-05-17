using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class UserTest
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public int TestId { get; set; }
        [ForeignKey("Id")]
        public int UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
    }
}
