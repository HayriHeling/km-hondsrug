using Eduria.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Controllers;

namespace Eduria.Models
{
    public class ShowTestModel
    {
        public List<CombinedQuestionAnswer> CombinedQuestionAnswers;

        public ShowTestModel(List<CombinedQuestionAnswer> combinedQuestionAnswers)
        {
            CombinedQuestionAnswers = combinedQuestionAnswers;
        }
    }
}
