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

        public ExamModel(List<QuestionModel> questionModels, List<AnswerModel> answerModels, string category)
        {
            this.AnswerModels = answerModels;
            this.QuestionModels = questionModels;
            this.Category = category;
        }

        public int CalculateTotalPoints()
        {
            return 0;
        }

        public void RemoveQuestion(CombinedQuestionAnswer question)
        {

        }
        public void AddQuestion()
        {

        }
        public void AddQuestion(CombinedQuestionAnswer question)
        {

        }
    }
}
