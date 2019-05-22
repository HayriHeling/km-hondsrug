using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class ExamModel
    {
        public int ExamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public List<QuestionModel> QuestionModels;
        public List<AnswerModel> AnswerModels;

        public List<AnswerModel> GetAnswersById(int questionId)
        {
            List<AnswerModel> answerList = new List<AnswerModel>();
            foreach(AnswerModel answer in AnswerModels)
            {
                if(answer.QuestionId == questionId)
                {
                    answerList.Add(answer);
                }
            }
            return answerList;
        }
    }
}
