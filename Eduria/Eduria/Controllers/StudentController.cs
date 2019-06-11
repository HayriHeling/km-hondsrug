using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using EduriaData.Models.ExamLayer;

namespace Eduria.Controllers
{
    public class StudentController : Controller
    {
        private ExamResultService ExamResultService { get; set; }
        private UserService UserService { get; set; }
        private ExamService ExamService { get; set; }
        private QuestionService QuestionService { get; set; }
        private AnswerService AnswerService { get; set; }
        private ExamQuestionService ExamQuestionService { get; set; }
        private TimeTableService TimeTableService { get; set; }

        public StudentController(ExamResultService examResultService, UserService userService, 
            ExamService examService, QuestionService questionService, AnswerService answerService,
            ExamQuestionService examQuestionService)
        {
            ExamResultService = examResultService;
            UserService = userService;
            ExamService = examService;
            QuestionService = questionService;
            AnswerService = answerService;
            ExamQuestionService = examQuestionService;
        }

        /// <summary>
        /// Default ActionResult
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Show the results from various tests in the database.
        /// </summary>
        /// <returns>An IActionResult that contains an IEnumerable<UserTest> with all its data.</returns>
        public IActionResult TestResults()
        {
            IEnumerable<ExamResult> examResults = ExamResultService.GetAll();
            IEnumerable<User> users = UserService.GetAll();
            IEnumerable<Exam> exams = ExamService.GetAll();
            IEnumerable<TimeTable> timeTables = TimeTableService.GetAll();

            var result = (from er in examResults
                          join u in users on er.UserId equals u.UserId
                          join e in exams on er.ExamId equals e.ExamId
                          join tb in timeTables on e.TimeTableId equals tb.TimeTableId

                          select new UserTestModel
                          {
                              Firstname = u.Firstname,
                              Lastname = u.Lastname,
                              ExamName = e.Name,
                              StartedAt = er.StartedAt,
                              FinishedAt = er.FinishedAt,
                              TimeTable = tb.Text,
                              Score = er.Score
                          });

            return View(result);
        }

        public IActionResult StudentExamResult(int studentId, int examId)
        {
            Exam exam = ExamService.GetById(studentId);
            ExamModel examModel = new ExamModel
            {
                AnswerModels = null,
                Description = exam.Description,
                ExamId = exam.ExamId,
                Name = exam.Name,
                QuestionModels = QuestionService.GetQuestionsByExamQuestionList(ExamQuestionService.GetAllQuestionIdsAsList(examId)),
            };
            return View(new ExamPerStudentModel{});
        }

        public List<QuestionModel> ConvertToQuestionModelList(List<Question> questions)
        {
            List<QuestionModel> questionModels = new List<QuestionModel>();
            foreach (Question question in questions)
            {
                questionModels.Add(new QuestionModel
                {
                    TimeTableId = question.TimeTableId,
                    MediaSourceId = question.M
                });
            }
        }
    }
}
