using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class ExamPerStudentModel
    {
        public ExamModel ExamModel { get; set; }
        public List<UserEQLogModel> UserEqLogModels { get; set; }
    }
}
