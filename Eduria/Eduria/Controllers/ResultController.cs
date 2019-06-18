using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models.ExamLayer;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class ResultController : Controller
    {
        private QuestionService QuestionService { get; set; }
        private ExamService ExamService { get; set; }
        private ExamQuestionService ExamQuestionService { get; set; }

        public ResultController(QuestionService questionService, ExamService examService, ExamQuestionService examQuestionService)
        {
            QuestionService = questionService;
            ExamService = examService;
            ExamQuestionService = examQuestionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Question()
        {
            IEnumerable<Question> questions = QuestionService.GetAll();
            IEnumerable<QuestionModel> questionModels = questions.Select(result => new QuestionModel
            {
                QuestionId = result.QuestionId,
                Text = result.Text
            });

            return View(questionModels);
        }

        public IActionResult Exam()
        {
            IEnumerable<Exam> exams = ExamService.GetAll();
            IEnumerable<ExamModel> examModels = exams.Select(result => new ExamModel
            {
                ExamId = result.ExamId,
                Name = result.Name,
                Description = result.Description
            });

            return View(examModels);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult QuestionResult(int id)
        {
            DataQuestionResultModel model = new DataQuestionResultModel
            {
                Name = ExamQuestionService.GetQuestionName(id),
                TimesWrong = ExamQuestionService.GetTotalTimesWrong(id),
                TimesAnswerd = ExamQuestionService.GetTotalAnswered(id),
                TimesGoodAtOnce = ExamQuestionService.GetTotalGoodAtOnce(id)
            };

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ExamResult(int id)
        {
            Exam exam = ExamService.GetById(id);

            //Adds the total per month
            List<int> totalPerMonth = new List<int>();
            for(int i = 1; i <= 12; i++)
            {
                totalPerMonth.Add(ExamService.GetTotalDoneBetweenDate(id, i));
            }

            DataExamResultModel model = new DataExamResultModel
            {
                Name = exam.Name,
                Description = exam.Description,
                TotalTimesDone = ExamService.GetTotalDone(id),
                TotalTimesDonePerMonth = totalPerMonth,
                HighestScore = ExamService.GetHighestScore(id),
                LowestScore = ExamService.GetLowestScore(id),
                AverageScore = ExamService.GetAverageScore(id)
            };

            return View(model);
        }
    }
}