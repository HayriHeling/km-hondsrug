using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using EduriaData.Models.ExamLayer;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class ResultController : Controller
    {
        private QuestionService QuestionService { get; set; }
        private ExamService ExamService { get; set; }
        private ExamQuestionService ExamQuestionService { get; set; }
        private UserService UserService { get; set; }
        private MediaSourceService MediaSourceService { get; set; }
        private TimeTableService TimeTableService { get; set; }
        public ResultController(QuestionService questionService, ExamService examService, ExamQuestionService examQuestionService, UserService userService, MediaSourceService mediaSourceService, TimeTableService timeTableService)
        {
            QuestionService = questionService;
            ExamService = examService;
            ExamQuestionService = examQuestionService;
            UserService = userService;
            MediaSourceService = mediaSourceService;
            TimeTableService = timeTableService;
        }

        public IActionResult Index()
        {
            IEnumerable<User> users = UserService.GetAll();
            IEnumerable<UserModel> userModels = users.Select(result => new UserModel
            {
                UserId = result.UserId,
                FirstName = result.Firstname,
                LastName = result.Lastname,
                UserNum = result.UserNum,
                ClassId = result.ClassId,
                UserType = (UserRoles)result.UserType
            });

            IEnumerable<Exam> exams = ExamService.GetAll();
            IEnumerable<ExamModel> examModels = exams.Select(result => new ExamModel
            {
                ExamId = result.ExamId,
                Name = result.Name,
                Description = result.Description
            });
            IEnumerable<Question> questions = QuestionService.GetAll();
            IEnumerable<QuestionModel> questionModels = questions.Select(result => new QuestionModel
            {
                QuestionId = result.QuestionId,
                MediaSourceModel = MediaSourceService.ConvertToModel(MediaSourceService.GetById(result.MediaSourceId)),
                QuestionType = result.QuestionType,
                Text = result.Text,
                TimeTableModel = ConvertToModel(TimeTableService.GetById(result.TimeTableId))
            });
            var tuple = Tuple.Create(userModels, examModels, questionModels);

            return View(tuple);
        }
        public TimeTableModel ConvertToModel(TimeTable timeTable)
        {
            return new TimeTableModel
            {
                TimeTableId = timeTable.TimeTableId,
                Text = timeTable.Text,
                MediaSourceModel = MediaSourceService.ConvertToModel(MediaSourceService.GetById(timeTable.MediaSourceId))
            };
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
        public IActionResult QuestionResult(int id, int examId)
        {
            DataQuestionResultModel model = new DataQuestionResultModel
            {
                Name = ExamQuestionService.GetQuestionName(id),
                TimesWrong = ExamQuestionService.GetTotalTimesWrong(id, examId),
                TimesAnswerd = ExamQuestionService.GetTotalTimesWrong(id) + ExamQuestionService.GetTotalTimesGood(id, examId),
                TimesGoodAtOnce = ExamQuestionService.GetTotalTimesGood(id, examId)
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
            ViewBag.examId = id;

            //Adds the total per month
            List<int> totalPerMonth = new List<int>();
            for(int i = 1; i <= 12; i++)
            {
                totalPerMonth.Add(ExamService.GetTotalDoneBetweenDate(id, i));
            }

            IEnumerable<Question> questions = ExamQuestionService.GetQuestionsByExamId(id);
            IEnumerable<QuestionModel> questionModels = questions.Select(result => new QuestionModel
            {
                QuestionId = result.QuestionId,
                Text = result.Text
            });

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

            IEnumerable<User> users = ExamService.GetUsersByExamId(id);
            IEnumerable<UserModel> userModels = users.Select(result => new UserModel
            {
                UserId = result.UserId,
                FirstName = result.Firstname,
                LastName = result.Lastname
            });

            var tuple = Tuple.Create(questionModels, model, userModels);

            return View(tuple);
        }
    }
}