using System.Collections.Generic;
using System.Linq;
using EduriaData.Models.ExamLayer;

namespace Eduria.Services
{
    public class QuestionService : AService<Question>
    {
        public QuestionService(EduriaContext context)
        {
            this.Context = context;
        }

        public override IEnumerable<Question> GetAll()
        {
            return Context.Questions;
        }

        public override Question GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.QuestionId == id);
        }

        /// <summary>
        /// Method to get all the questions by the examQuestions-list.
        /// </summary>
        /// <param name="examQuestions">List of examQuestions</param>
        /// <returns>List of Question-models</returns>
        public IEnumerable<Question> GetQuestionsByExamQuestionList(IEnumerable<ExamQuestion> examQuestions)
        {
            List<Question> questions = new List<Question>();
            foreach (ExamQuestion eq in examQuestions)
            {
                IEnumerable<Question> tempQuestions = Context.Questions.Where(x => x.QuestionId == eq.QuestionId);
                foreach (Question question in tempQuestions)
                {
                    questions.Add(question);
                }
            }

            return questions;
        }

        public Question GetQuestionByText(string text)
        {
            return Context.Questions.FirstOrDefault(x => x.Text == text);
        }
        public Question GetQuestionByMediaLink(string text)
        {
            int mediaSourceId = Context.MediaSources.FirstOrDefault(x => x.Source == text).MediaSourceId;
            return Context.Questions.FirstOrDefault(x => x.MediaSourceId == mediaSourceId);
        }

    }
}