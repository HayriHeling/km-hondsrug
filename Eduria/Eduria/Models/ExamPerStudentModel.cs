using System.Collections.Generic;

namespace Eduria.Models
{
    public class ExamPerStudentModel
    {
        public ExamModel ExamModel { get; set; }
        public List<UserEQLogModel> UserEqLogModels { get; set; }
    }
}
