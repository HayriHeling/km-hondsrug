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
        private UserEQLogService UserEqLogService { get; set; }
        private MediaSourceService MediaSourceService { get; set; }

        public StudentController(ExamResultService examResultService, UserService userService, 
            ExamService examService, QuestionService questionService, AnswerService answerService,
            ExamQuestionService examQuestionService, TimeTableService timeTableService, 
            UserEQLogService userEqLogService, MediaSourceService mediaSourceService)
        {
            ExamResultService = examResultService;
            UserService = userService;
            ExamService = examService;
            QuestionService = questionService;
            AnswerService = answerService;
            ExamQuestionService = examQuestionService;
            TimeTableService = timeTableService;
            UserEqLogService = userEqLogService;
            MediaSourceService = mediaSourceService;
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
                              ExamResultId = er.ExamResultId,
                              Firstname = u.Firstname,
                              Lastname = u.Lastname,
                              ExamName = e.Name,
                              StartedAt = er.StartedAt,
                              FinishedAt = er.FinishedAt,
                              TimeTableModel = ConvertToTimeTableModel(TimeTableService.GetById(tb.TimeTableId)),
                              Score = er.Score
                          });

            return View(result);
        }

        /// <summary>
        /// Method that gets fired when a user goes to StudentExamResult. The id represents an examresultId
        /// </summary>
        /// <param name="id">The ExamResultId to get an examresult.</param>
        /// <returns>A View with an ExamResultModel</returns>
        public IActionResult StudentExamResult(int id)
        {
            ExamResultModel examResultModel = GetExamResultModelById(id);
            Exam exam = ExamService.GetById(examResultModel.UserId);
            ExamModel examModel = CreateExamModel(exam);
            return View(new ExamPerStudentModel
            {
                ExamModel = examModel,
                UserEqLogModels = CreateUserEqLogModels(id)
            });
        }

        /// <summary>
        /// Method that creates an ExamModel with data from an Exam model out of the EduriaContext namespace
        /// </summary>
        /// <param name="exam">The EduriaContext Exam Model </param>
        /// <returns>An ExamModel object</returns>
        private ExamModel CreateExamModel(Exam exam)
        {
            List<Question> questions = QuestionService
                .GetQuestionsByExamQuestionList(ExamQuestionService.GetAllQuestionIdsAsList(exam.ExamId)).ToList();
            TimeTable timeTable = TimeTableService.GetById(exam.TimeTableId);
            List<Answer> answers = AnswerService.GetAnswersByQuestionsList(questions).ToList();

            List<QuestionModel> questionModels = ConvertToQuestionModelList(questions);
            List<AnswerModel> answerModels = ConvertToAnswerModelList(answers);

            return new ExamModel
            {
                AnswerModels = answerModels,
                Description = exam.Description,
                ExamId = exam.ExamId,
                Name = exam.Name,
                QuestionModels = questionModels,
                TimeTable = ConvertToTimeTableModel(timeTable)
            };
        }

        /// <summary>
        /// Method that gets the ExamResultModel by its ExamResultId.
        /// </summary>
        /// <param name="examResultId">The ExamResultId of the ExamResult</param>
        /// <returns>An ExamResultModel</returns>
        private ExamResultModel GetExamResultModelById(int examResultId)
        {
            ExamResult examResult = ExamResultService.GetById(examResultId);
            return new ExamResultModel
            {
                ExamId = examResult.ExamId,
                ExamResultId = examResult.ExamResultId,
                FinishedAt = examResult.FinishedAt,
                Score = examResult.Score,
                StartedAt = examResult.StartedAt,
                UserId = examResult.UserId
            };
        }

        /// <summary>
        /// Method that converts EduriaContext Questions to a list of QuestionModels.
        /// </summary>
        /// <param name="questions">The Question objects from the Eduria Context namespace</param>
        /// <returns>A list of QuestionModels</returns>
        private List<QuestionModel> ConvertToQuestionModelList(List<Question> questions)
        {
            List<QuestionModel> questionModels = new List<QuestionModel>();
            foreach (Question question in questions)
            {
                questionModels.Add(new QuestionModel
                {
                    TimeTableModel = ConvertToTimeTableModel(TimeTableService.GetById(question.TimeTableId)),
                    MediaSourceModel = ConvertToMediaSourceModel(MediaSourceService.GetById(question.MediaSourceId)),
                    QuestionId = question.QuestionId,
                    QuestionType = question.QuestionType,
                    Text = question.Text
                });
            }

            return questionModels;
        }

        /// <summary>
        /// Method to convert a list of Answers from the EduriaContext namespace to a list of Answermodels.
        /// </summary>
        /// <param name="answers">The list of Answers from the EduriaContext namespace</param>
        /// <returns>A list of Answermodels</returns>
        private List<AnswerModel> ConvertToAnswerModelList(List<Answer> answers)
        {
            List<AnswerModel> answerModels = new List<AnswerModel>();
            foreach (Answer answer in answers)
            {
                answerModels.Add(new AnswerModel
                {
                    AnswerId = answer.AnswerId,
                    CorrectAnswer = answer.Correct.Equals(1),
                    QuestionId = answer.QuestionId,
                    Text = answer.Text
                });
            }

            return answerModels;
        }

        /// <summary>
        /// Method that creates a list of UserEQLogModels from an examresultid.
        /// </summary>
        /// <param name="examResultId">The examResultId</param>
        /// <returns>The list of UserEQLogModels</returns>
        private List<UserEQLogModel> CreateUserEqLogModels(int examResultId)
        {
            List<UserEQLog> userEqLogs = UserEqLogService.GetAllByResultId(examResultId).ToList();
            List<UserEQLogModel> userEqLogModels = new List<UserEQLogModel>();

            foreach (UserEQLog userEqLog in userEqLogs)
            {
                userEqLogModels.Add(new UserEQLogModel
                {
                    AnsweredOn = userEqLog.AnsweredOn,
                    CorrectAnswered = userEqLog.CorrectAnswered,
                    ExamHasQuestionId = userEqLog.ExamHasQuestionId,
                    ExamResultId = userEqLog.ExamResultId,
                    TimesWrong = userEqLog.TimesWrong,
                    UserEQLogId = userEqLog.UserEQLogId,
                    UserId = userEqLog.UserId
                });
            }

            return userEqLogModels;
        }
        public MediaSourceModel ConvertToMediaSourceModel(MediaSource mediaSource)
        {
            return new MediaSourceModel
            {
                MediaSourceId = mediaSource.MediaSourceId,
                MediaType = (MediaType)mediaSource.MediaType,
                Source = mediaSource.Source
            };
        }

        public TimeTableModel ConvertToTimeTableModel(TimeTable timeTable)
        {
            return new TimeTableModel
            {
                TimeTableId = timeTable.TimeTableId,
                MediaSourceModel = ConvertToMediaSourceModel(MediaSourceService.GetById(timeTable.MediaSourceId)),
                Text = timeTable.Text,
                Description = timeTable.Description,
                TimeTableDesignId = timeTable.TimeTableDesignId
            };
        }
    }
}
