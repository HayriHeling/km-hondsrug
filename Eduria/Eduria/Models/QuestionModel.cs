using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Models
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public MediaType Type { get; set; }
        public string Link { get; set; }

        public QuestionModel()
        {

        }

        public QuestionModel(int id, string text, MediaType mediaType=MediaType.None, string link="")
        {
            this.QuestionId = id;
            this.Text = text;
        }
        public IActionResult GetView()
        {
            throw new NotImplementedException();
        }
    }
}
