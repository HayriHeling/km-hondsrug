using Eduria.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class TestModel
    {
        public List<CombinedQuestionAnswer> CombinedQuestionAnswers;

        public TestModel()
        {
            throw new NotImplementedException();
        }

        public bool CheckAnswer(int id, int givenAnswer = 0, string givenAnswerString = "")
        {
            return true;
        }

        public void InsertQuestion(int id)
        {
            CombinedQuestionAnswers.Insert(1, new CombinedQuestionAnswer());
        }
    }
}
