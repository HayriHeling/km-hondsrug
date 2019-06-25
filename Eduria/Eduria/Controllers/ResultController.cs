using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private UserEQLogService UserEQLogService { get; set; }
        private ExamResultService ExamResultService { get; set; }

        public ResultController(QuestionService questionService, ExamService examService, 
            ExamQuestionService examQuestionService, UserService userService, MediaSourceService mediaSourceService, 
            TimeTableService timeTableService, UserEQLogService userEQLogService, ExamResultService examResultSerivce)
        {
            QuestionService = questionService;
            ExamService = examService;
            ExamQuestionService = examQuestionService;
            UserService = userService;
            MediaSourceService = mediaSourceService;
            TimeTableService = timeTableService;
            UserEQLogService = userEQLogService;
            ExamResultService = examResultSerivce;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                TimeTableModel = ConvertToTimeTableModel(TimeTableService.GetById(result.TimeTableId))
            });
            IEnumerable<UserEQLog> logs = UserEQLogService.GetAll();
            IEnumerable<UserEQLogModel> logModels = logs.Select(result => new UserEQLogModel
            {
                UserEQLogId = result.UserEQLogId,
                ExamHasQuestionId = result.ExamHasQuestionId,
                ExamResultId = result.ExamResultId,
                QuestionModel = ConvertToQuestionModel(QuestionService.GetById(ExamQuestionService.GetById(result.ExamHasQuestionId).QuestionId)),
                UserId = result.UserId,
                TimesWrong = result.TimesWrong,
                AnsweredOn = result.AnsweredOn,
                CorrectAnswered = result.CorrectAnswered
            });
            var tuple = Tuple.Create(userModels, examModels, questionModels, logModels);

            return View(tuple);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public QuestionModel ConvertToQuestionModel(Question question)
        {
            return new QuestionModel
            {
                QuestionId = question.QuestionId,
                Text = question.Text,
                QuestionType = question.QuestionType,
                MediaSourceModel = MediaSourceService.ConvertToModel(MediaSourceService.GetById(question.MediaSourceId)),
                TimeTableModel = ConvertToTimeTableModel(TimeTableService.GetById(question.TimeTableId))
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeTable"></param>
        /// <returns></returns>
        public TimeTableModel ConvertToTimeTableModel(TimeTable timeTable)
        {
            return new TimeTableModel
            {
                TimeTableId = timeTable.TimeTableId,
                Text = timeTable.Text,
                MediaSourceModel = MediaSourceService.ConvertToModel(MediaSourceService.GetById(timeTable.MediaSourceId)),
                Description = timeTable.Description,
                TimeTableDesignId = timeTable.TimeTableDesignId
            };
        }
        public UserModel ConvertToUserModel(User user)
        {
            return new UserModel
            {
                FirstName = user.Firstname,
                LastName = user.Lastname
            };
        }
        public ExamModel ConvertToExamModel(Exam exam)
        {
            return new ExamModel
            {
                Name = exam.Name,
                Description = exam.Description
            };
        }
        public ExamResultModel ConvertToExamResultModel(ExamResult er)
        {
            return new ExamResultModel
            {
                ExamResultId = er.ExamResultId,
                ExamId = er.ExamId,
                UserId = er.UserId,
                StartedAt = er.StartedAt,
                FinishedAt = er.FinishedAt,
                Score = er.Score
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        public IActionResult QuestionResult(int id, int examId = -1)
        {
            DataQuestionResultModel model;
            if (examId == -1)
            {
                model = new DataQuestionResultModel
                {
                    Name = QuestionService.GetById(id).Text,
                    TimesWrong = ExamQuestionService.GetTotalTimesWrong(id),
                    TimesAnswerd = ExamQuestionService.GetTotalTimesWrong(id) + ExamQuestionService.GetTotalTimesGood(id),
                    TimesGoodAtOnce = ExamQuestionService.GetTotalTimesGood(id)
                };
            }
            else
            {
                model = new DataQuestionResultModel
                {
                    Name = ExamQuestionService.GetQuestionName(id),
                    TimesWrong = ExamQuestionService.GetTotalTimesWrong(id, examId),
                    TimesAnswerd = ExamQuestionService.GetTotalTimesWrong(id) + ExamQuestionService.GetTotalTimesGood(id, examId),
                    TimesGoodAtOnce = ExamQuestionService.GetTotalTimesGood(id, examId)
                };
            }

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

        /// <summary>
        /// Action for returning the specified models.
        /// </summary>
        /// <returns></returns>
        public IActionResult StudentResult(int id)
        {
            int userId;

            if (!User.Identity.IsAuthenticated)
            {
                userId = id;
            }
            else
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            
            IEnumerable<Question> question = ExamQuestionService.GetQuestionsByUserId(userId);
            IEnumerable<QuestionModel> questionModels = question.Select(result => new QuestionModel
            {
                QuestionId = result.QuestionId,
                QuestionType = result.QuestionType,
                Text = result.Text
            });

            IEnumerable<UserEQLog> logs = UserEQLogService.GetAllByUserId(userId);
            IEnumerable<UserEQLogModel> logModels = logs.Select(result => new UserEQLogModel
            {
                UserEQLogId = result.UserEQLogId,
                ExamHasQuestionId = result.ExamHasQuestionId,
                ExamResultId = result.ExamResultId,
                QuestionModel = ConvertToQuestionModel(QuestionService.GetById(ExamQuestionService.GetById(result.ExamHasQuestionId).QuestionId)),
                UserId = result.UserId,
                TimesWrong = result.TimesWrong,
                AnsweredOn = result.AnsweredOn,
                CorrectAnswered = result.CorrectAnswered
            });

            var tuple = Tuple.Create(ExamResultService.GetDataStudentResultByUserId(userId), questionModels, logModels);

            return View(tuple);
        }
        public IActionResult StudentExamResult(int id)
        {
            List<QuestionModel> QuestionsRight = new List<QuestionModel>();
            List<QuestionModel> QuestionsWrong = new List<QuestionModel>();
            List<string> dates = new List<string>();
            List<int> scores = new List<int>();

            ExamResultModel resultModel = ConvertToExamResultModel(ExamResultService.GetById(id));
            UserModel user = ConvertToUserModel(UserService.GetById(resultModel.UserId));

            IEnumerable<ExamResult> examResults = ExamResultService.GetExamResultByUserId(resultModel.UserId);
            IEnumerable<ExamResultModel> examResultModels = examResults.Select(result => new ExamResultModel
            {
                ExamId = result.ExamId,
                ExamResultId = result.ExamResultId,
                StartedAt = result.StartedAt,
                FinishedAt = result.FinishedAt,
                Score = result.Score,
                UserId = result.UserId
            });          
            foreach(ExamResultModel result in examResultModels)
            {
                dates.Add(result.FinishedAt.ToString());
                scores.Add(result.Score);
            }
            IEnumerable<UserEQLog> examLogs = UserEQLogService.GetAllByResultId(id);
            foreach(UserEQLog examLog in examLogs)
            {
                Question question = QuestionService.GetById(ExamQuestionService.GetQuestionIdByExamQuestionId(examLog.ExamHasQuestionId));
                QuestionModel questionModel = ConvertToQuestionModel(question);
                if (examLog.CorrectAnswered == 1)
                {
                    QuestionsRight.Add(questionModel);
                }
                else
                {
                    QuestionsWrong.Add(questionModel);
                }
            }
            ViewBag.User = user;
            ViewBag.Exam = ConvertToExamModel(ExamService.GetById(ExamResultService.GetById(id).ExamId));
            ViewBag.Result = resultModel;
            ViewBag.AllResults = examResultModels;
            ViewBag.QuestionsRight = QuestionsRight;
            ViewBag.QuestionsWrong = QuestionsWrong;
            return View();
        }
    }
}