using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Models
{
    public class CombinedQuestionAnswer
    {
        public IQuestion QuestionModel { get; set; }
        public AnswerModel AnswerModel { get; set; }
        public CombinedQuestionAnswer()
        {
            QuestionModel = new TextQuestionModel();
            AnswerModel = new AnswerModel();
        }
    }

    public class TextQuestionModel : IQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public TextQuestionModel()
        {

        }

        public TextQuestionModel(int id, string text)
        {
            this.Id = id;
            this.Text = text;
        }
        public IActionResult GetView()
        {
            throw new NotImplementedException();
        }
    }

    public class AnswerModel
    {
        public int Id;
        public string Text;
        public bool CorrectAnswer;

        public AnswerModel()
        {

        }

        public AnswerModel(int id, string text, bool correctAnswer)
        {
            this.Id = id;
            this.Text = text;
            this.CorrectAnswer = correctAnswer;
        }
    }
}
