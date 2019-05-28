using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eduria.JsonClasses;
using Newtonsoft.Json.Linq;

namespace Eduria.Controllers
{
    public class ExamController : Controller
    {
        private ExamService _examService;
        private QuestionService _questionService;
        private AnswerService _answerService;
        private QuestionHasAnswerTService _questionHasAnswerTService;
        private ExamQuestionService _examQuestionService;

        public ExamController(ExamService examService, QuestionService questionService, AnswerService answerService, QuestionHasAnswerTService questionHasAnswerTService, ExamQuestionService examQuestionService)
        {
            this._examQuestionService = examQuestionService;
            this._examService = examService;
            this._questionService = questionService;
            this._questionHasAnswerTService = questionHasAnswerTService;
            //this._answerService = answerService;
        }

        /// <summary>
        /// Method that creates a view for an exammodel.
        /// </summary>
        /// <param name="id">Id of the exammodel</param>
        /// <returns>View of the exammodel</returns>
        public IActionResult Show(int id=1)
        {
            //return View(GetExamDataById(id));
            return View(GetExamModelByExamId(id));
        }

        public IActionResult SendResults(string jsoninput)
        {
            List<UserEqLogJson> userEqLogJsons = new List<UserEqLogJson>();
            var objects = JArray.Parse(jsoninput);
            foreach (JObject root in objects)
            {
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    Debug.WriteLine(app.Value);
                }
            }
            Debug.WriteLine("Done");
            return View(null);
        }

        /// <summary>
        /// Function that returns an ExamModel by the id of the exam.
        /// </summary>
        /// <param name="id">Examid</param>
        /// <returns>An complete ExamModel</returns>
        public ExamModel GetExamModelByExamId(int id)
        {
            IEnumerable<ExamQuestion> tempExamQuestions = _examQuestionService.GetAllQuestionIdsAsList(id);
            IEnumerable<Question> tempQuestions = _questionService.GetQuestionsByExamQuestionList(tempExamQuestions);
            //IEnumerable<Answer> tempAnswers = _answerService.GetAnswersByQuestionsList(tempQuestions);

            return new ExamModel()
            {
                AnswerModels = null, //CreateAnswerModels(tempAnswers.ToList()),
                Category = "",
                Description = "",
                ExamId = id,
                Name = "None",
                QuestionModels = CreateQuestionModelsList(tempQuestions.ToList())
            };
        }

        /// <summary>
        /// Gets the questionmodels and answermodels which belong to the examId.
        /// </summary>
        /// <param name="id">The examId</param>
        /// <returns>a complete Exammodel</returns>
        public ExamModel GetExamDataById(int id)
        {
            List<Question> allQuestions = _questionService.GetAll().ToList();
            List<Answer> allAnswers = _answerService.GetAll().ToList();

            //List<AnswerModel> answerModels = CreateAnswerModels(allAnswers);
            List<QuestionModel> questionModels = CreateQuestionModelsList(allQuestions);

            return new ExamModel()
            {
                AnswerModels = null,
                Category = "",
                Description = "",
                ExamId = 0,
                Name = "None",
                QuestionModels = questionModels
            };
        }

        /// <summary>
        /// Creates a list of QuestionModel-models from a list of Question-models
        /// </summary>
        /// <param name="questions">List of Question-models</param>
        /// <returns>A list of QuestionModel-models</returns>
        public List<QuestionModel> CreateQuestionModelsList(List<Question> questions)
        {
            List<QuestionModel> outputList = new List<QuestionModel>();
            foreach (Question question in questions)
            {
                outputList.Add(new QuestionModel()
                {
                    Category = question.CategoryId.ToString(),
                    MediaLink = question.MediaLink,
                    MediaType = 0,
                    QuestionId = question.QuestionId,
                    Text = question.Text,
                    AnswerId = _questionHasAnswerTService.GetByQuestionId(question.QuestionId).AnswerTId
            });
            }

            return outputList;
        }

        /// <summary>
        /// Creates a list of Answermodel-models from a list of Answer-models
        /// </summary>
        /// <param name="answers">List of answer-models</param>
        /// <returns>List of Answermodel-models</returns>
        public List<AnswerModel> CreateAnswerModels(List<Answer> answers)
        {
            List<AnswerModel> tempAnswerModels = new List<AnswerModel>();
            foreach (Answer answer in answers)
            {
                tempAnswerModels.Add(new AnswerModel()
                {
                    AnswerId = answer.AnswerId,
                    CorrectAnswer = answer.Correct.Equals(1),
                    QuestionId = answer.QuestionId,
                    Text = answer.Text
                });
            }

            return tempAnswerModels;
        }
    }
}