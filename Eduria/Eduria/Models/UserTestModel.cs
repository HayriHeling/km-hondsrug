using EduriaData.Models;
using System;

namespace Eduria.Models
{
    /// <summary>
    /// UserTest model with the Test and User data.
    /// </summary>
    public class UserTestModel
    {
        public int Id { get; set; }
        public Test Test { get; set; }
        public User User { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
    }
}
