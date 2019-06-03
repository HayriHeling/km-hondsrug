using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eduria.JsonClasses;
using Eduria.Models;
using Eduria.Services;
using Microsoft.AspNetCore.Mvc;
using EduriaData.Models.ExamLayer;
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
        private UserEQLogService _userEqLogService;
        private ExamResultService _examResultService;

        private int _examId;
        private DateTime _dateTime;

        public ExamController(ExamService examService, QuestionService questionService, 
            QuestionHasAnswerTService questionHasAnswerTService, 
            ExamQuestionService examQuestionService, UserEQLogService userEqLogService,
            ExamResultService examResultService)
        {
            this._examQuestionService = examQuestionService;
            this._examService = examService;
            this._questionService = questionService;
            this._questionHasAnswerTService = questionHasAnswerTService;
            this._userEqLogService = userEqLogService;
            this._examResultService = examResultService;
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
            _examId = id;
            _dateTime = DateTime.Now;

            return View(GetExamModelByExamId(id));
        }

        /// <summary>
        /// Method that creates an overview for all exams.
        /// </summary>
        /// <returns>View for all exams</returns>
        public IActionResult OverView()
        {
            return View(_examService.GetAll());
        }

        /// <summary>
        /// Method used for sending the data from the exam to this controller
        /// </summary>
        /// <param name="jsoninput">Json data</param>
        /// <param name="examId"></param>
        /// <param name="userId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public IActionResult SendResults(string jsoninput, int examId, int userId, int score, DateTime starttime, DateTime endtime)
        {
            Debug.WriteLine("Done");
            ImportExamResultToDatabase(examId, userId, score, starttime, endtime);
            ImportQuestionsToDatabase(CreatEqLogJsonsFromJson(jsoninput), examId, userId);
            return View(null);
        }

        /// <summary>
        /// Method that returns a list of UserEQLogJson objects from a string of json
        /// </summary>
        /// <param name="jsoninput">The json string</param>
        /// <returns>A list of UserEQLogJson objects</returns>
        public List<UserEqLogJson> CreatEqLogJsonsFromJson(string jsoninput)
        {
            List<UserEqLogJson> userEqLogJsons = new List<UserEqLogJson>();
            JArray objects = JArray.Parse(jsoninput);
            foreach (var jToken in objects)
            {
                userEqLogJsons.Add(new UserEqLogJson()
                {
                    AnsweredOn = jToken.First.First["AnsweredOn"].ToObject<DateTime>(),
                    CorrectAnswered = jToken.First.First["CorrectAnswered"].ToObject<int>(),
                    QuestionId = jToken.First.First["QuestionId"].ToObject<int>(),
                    TimesWrong = jToken.First.First["TimesWrong"].ToObject<int>()
                });
            }

            return userEqLogJsons;
        }

        /// <summary>
        /// Function that imports UserEQLog objects in the database.
        /// </summary>
        /// <param name="userEqLogJsons">A list of UserEqLogJson objects</param>
        /// <param name="examId"></param>
        /// <param name="userId"></param>
        public void ImportQuestionsToDatabase(List<UserEqLogJson> userEqLogJsons, int examId, int userId)
        {
            foreach (UserEqLogJson userEqLogJson in userEqLogJsons)
            {
                UserEQLog userEqLog = new UserEQLog()
                {
                    AnsweredOn = userEqLogJson.AnsweredOn,
                    CorrectAnswered = userEqLogJson.CorrectAnswered,
                    ExamHasQuestionId = _examQuestionService.GetExamQuestionByQuestionIdExamId(userEqLogJson.QuestionId, examId).ExamHasQuestionId,
                    ExamResultId = _examResultService.GetExamResultByUserAndExamId(userId:1, examId).ExamResultId,
                    TimesWrong = userEqLogJson.TimesWrong,
                    UserId = userId
                };
                _userEqLogService.Add(userEqLog);
            }
        }

        /// <summary>
        /// Function that imports the result of the exam in the database.
        /// </summary>
        public void ImportExamResultToDatabase(int examId, int userId, int score, DateTime start, DateTime end)
        {
            _examResultService.Add(new ExamResult()
            {
                ExamId = examId,
                StartedAt = start,
                FinishedAt = end,
                UserId = userId,
                Score = score
            });
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
            Exam exam = _examService.GetById(id);

            return new ExamModel()
            {
                AnswerModels = null, //CreateAnswerModels(tempAnswers.ToList()),
                Category = exam.CategoryId.ToString(),
                Description = exam.Description,
                ExamId = id,
                Name = exam.Name,
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
            //List<Answer> allAnswers = _answerService.GetAll().ToList();
            Exam exam = _examService.GetById(id);

            //List<AnswerModel> answerModels = CreateAnswerModels(allAnswers);
            List<QuestionModel> questionModels = CreateQuestionModelsList(allQuestions);

            return new ExamModel()
            {
                AnswerModels = null,
                Category = exam.CategoryId.ToString(),
                Description = exam.Description,
                ExamId = id,
                Name = exam.Name,
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