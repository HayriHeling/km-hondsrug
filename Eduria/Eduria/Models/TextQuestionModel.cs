using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Models
{
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
}
