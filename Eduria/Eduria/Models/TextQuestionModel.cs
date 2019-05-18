using Eduria.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class TextQuestionModel : IQuestion
    {
        public int Id;
        public string Text;
        public List<AnswerModel> answerModels;

        public TextQuestionModel(int id)
        {

        }
        public IActionResult GetView()
        {
            throw new NotImplementedException();
        }
    }
}
