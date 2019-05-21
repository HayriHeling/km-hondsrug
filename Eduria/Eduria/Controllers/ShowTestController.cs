using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class ShowTestController : Controller
    {
        public List<CombinedQuestionAnswer> CombinedQuestionAnswers;

        private int MinRand = 3;
        private int MaxRand = 6;

        public IActionResult Index()
        {
            return View();
        }

        private CombinedQuestionAnswer ShowNextCombinedQuestionAnswer()
        {
            CombinedQuestionAnswer tempQuestionAnswer = CombinedQuestionAnswers[0];
            RemoveQa(tempQuestionAnswer);
            return tempQuestionAnswer;
        }

        private bool RemoveQa(CombinedQuestionAnswer combinedQuestionAnswer)
        {
            return CombinedQuestionAnswers.Remove(combinedQuestionAnswer);
        }

        public bool CheckAnswer(int id, int givenAnswer = 0, string givenAnswerString = "")
        {
            return true;
        }

        public void InsertQuestion(CombinedQuestionAnswer combinedQuestionAnswer)
        {
            int x = RandomNo();
            if (CombinedQuestionAnswers.Count >= MaxRand)
            {
                x = CombinedQuestionAnswers.Count - 1;
            }
            CombinedQuestionAnswers.Insert(x, combinedQuestionAnswer);
        }

        public int RandomNo()
        {
            Random random = new Random();
            return random.Next(MinRand, MaxRand);
        }
    }
}