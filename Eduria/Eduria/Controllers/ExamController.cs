using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eduria.JsonClasses;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using System.Web;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using EduriaData.Models.ExamLayer;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Eduria.Controllers
{
    public class ExamController : Controller
    {
        private ExamService _examService;
        private QuestionService _questionService;
        private AnswerService _answerService;
        private TimeTableService _timeTableService;
        private ExamQuestionService _examQuestionService;
        
        /// <summary>
        /// internal class for databinding the information from database to object.
        /// </summary>
        [DataContract]
        class exam
        {
            [DataMember]
            public string name;
            [DataMember]
            public string description;
            [DataMember]
            public int category;
            [DataMember]
            public question[] questions;
        }
        /// <summary>
        /// internal class for databinding the information from database to object.
        /// </summary>
        [DataContract]
        class question
        {
            [DataMember]
            public int id;
            [DataMember]
            public int answerCount;
            [DataMember]
            public string text;
            [DataMember]
            public int category;
            [DataMember]
            public int mediaType;
            [DataMember]
            public string mediaLink;
            [DataMember]
            public answer[] answers;
            [DataMember]
            public bool existing;
            [DataMember]
            public int questionType;
        }
        /// <summary>
        /// internal class for databinding the information from database to object.
        /// </summary>
        [DataContract]
        class answer
        {
            [DataMember]
            public int id;
            [DataMember]
            public int questionId;
            [DataMember]
            public string text;
            [DataMember]
            public int correct;
        }

        private UserEQLogService _userEqLogService;
        private ExamResultService _examResultService;

        private int _examId;
        private DateTime _dateTime;

        /// <summary>
        /// Constructor that creates all needed services.
        /// </summary>
        /// <param name="examService"></param>
        /// <param name="questionService"></param>
        /// <param name="answerService"></param>
        /// <param name="timeTableService"></param>
        /// <param name="examQuestionService"></param>
        /// <param name="userEqLogService"></param>
        /// <param name="examResultService"></param>
        public ExamController(ExamService examService, QuestionService questionService, AnswerService answerService,
            TimeTableService timeTableService, ExamQuestionService examQuestionService, 
            UserEQLogService userEqLogService, ExamResultService examResultService)
        {
            this._examQuestionService = examQuestionService;
            this._examService = examService;
            this._questionService = questionService;
            this._timeTableService = timeTableService;
            this._userEqLogService = userEqLogService;
            this._examResultService = examResultService;
            this._answerService = answerService;
        }

        /// <summary>
        /// Method that creates a view for an exammodel.
        /// </summary>
        /// <param name="id">Id of the exammodel</param>
        /// <returns>View of the exammodel</returns>
        public IActionResult Show(int id = 1)
        {
            //return View(GetExamDataById(id));
            _examId = id;
            _dateTime = DateTime.Now;

            return View(GetExamModelByExamId(id));
        }

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
                    ExamResultId = _examResultService.GetExamResultByUserAndExamId(userId: 1, examId).ExamResultId,
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
            Exam exam = _examService.GetById(id);
            TimeTable timeTable = _timeTableService.GetById(exam.TimeTableId);
            return new ExamModel()
            {
                AnswerModels = null, //CreateAnswerModels(tempAnswers.ToList()),
                TimeTable = new TimeTableModel()
                {
                    TimeTableId = timeTable.TimeTableId,
                    Text = timeTable.Text,
                    Source = timeTable.Source
                },
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
            Exam exam = _examService.GetById(id);
            TimeTable timeTable = _timeTableService.GetById(exam.TimeTableId);
            //List<AnswerModel> answerModels = CreateAnswerModels(allAnswers);
            List<QuestionModel> questionModels = CreateQuestionModelsList(allQuestions);          
            return new ExamModel()
            {
                AnswerModels = null,
                TimeTable = new TimeTableModel()
                {
                    TimeTableId = timeTable.TimeTableId,
                    Text = timeTable.Text,
                    Source = timeTable.Source
                },
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
                TimeTable tt = _timeTableService.GetById(question.TimeTableId);
                TimeTableModel ttModel = new TimeTableModel()
                {
                    TimeTableId = tt.TimeTableId,
                    Text = tt.Text,
                    Source = tt.Source
                };
                outputList.Add(new QuestionModel()
                {
                    TimeTable = ttModel,
                    MediaLink = question.MediaLink,
                    MediaType = (MediaType)question.MediaType,
                    QuestionId = question.QuestionId,
                    Text = question.Text,
                    AnswerId = question.TimeTableId
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

        /// <summary>
        /// Gets tables from database and changes them to models to use in the view. Returns the view to create an exam.
        /// if success is true, the view will display a message that an exam is successfully created.
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        //GET: Exam/Create
        public ActionResult Create(int success = 0)
        {
            ViewBag.Success = success;

            IEnumerable<TimeTable> tables = _timeTableService.GetAll();
            List<TimeTableModel> tableModels = new List<TimeTableModel>();
            foreach(TimeTable table in tables)
            {
                TimeTableModel tableModel = new TimeTableModel()
                {
                    TimeTableId = table.TimeTableId,
                    Text = table.Text,
                    Source = table.Source
                };
                tableModels.Add(tableModel);
            }
            ViewBag.categories = tableModels;

            IEnumerable<Question> questions = _questionService.GetAll();
            List<QuestionModel> questionModels = new List<QuestionModel>();
            foreach (Question question in questions)
            {
                QuestionModel questionModel = new QuestionModel()
                {
                    QuestionId = question.QuestionId,
                    QuestionType = question.QuestionType,
                    TimeTable = tableModels.First(x => x.TimeTableId == question.TimeTableId),
                    Text = question.Text,
                    MediaType = (MediaType)question.MediaType,
                    MediaLink = question.MediaLink
                };
                questionModels.Add(questionModel);
            }
            ViewBag.questions = questionModels;
            return View();
        }
        /// <summary>
        /// Creates an exam and saves all attributes to the database.
        /// </summary>
        /// <param name="examJson"> The complete exam in json format.</param>
        /// <returns></returns>
        public ActionResult CreateExam(string examJson)
        {
            string json = examJson;
            exam exam;
            try
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    // Deserialization from JSON  
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(exam));
                    exam = (exam)deserializer.ReadObject(ms);
                }

                Exam ex = new Exam()
                {
                    TimeTableId = exam.category,
                    Name = exam.name,
                    Description = exam.description
                };
                _examService.Add(ex);
                int examId = _examService.GetByName(ex.Name).ExamId;

                foreach(question q in exam.questions)
                {
                    int _questionId;
                    if (!q.existing)
                    {
                        Question question = new Question()
                        {
                            Text = q.text,
                            QuestionType = q.questionType,
                            TimeTableId = q.category,
                            MediaType = q.mediaType,
                            MediaLink = q.mediaLink
                        };
                        _questionService.Add(question);
                        _questionId = _questionService.GetQuestionByText(question.Text).QuestionId;
                    }      
                    else
                    {
                        _questionId = _questionService.GetQuestionByText(q.text).QuestionId;
                    }

                    ExamQuestion eq = new ExamQuestion()
                    {
                        ExamId = examId,
                        QuestionId = _questionId
                    };

                    _examQuestionService.Add(eq);

                    foreach (answer a in q.answers)
                    {
                        Answer answer = new Answer()
                        {
                            QuestionId = _questionId,
                            Text = a.text,
                            Correct = a.correct
                        };
                        _answerService.Add(answer);
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return RedirectToAction("Create", "Exam");
        }            
        /// <summary>
        /// Gets called if files are being uploaded when creating a new exam. Files are being saved in the content folder and the question gets a reference.
        /// </summary>
        /// <param name="files">The files that needs to be uploaded to the server.</param>
        /// <returns></returns>
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var filePath = "lol";

            foreach(var formFile in files)
            {
                if(formFile.Length > 0)
                {
                    Question q = _questionService.GetQuestionByMediaLink(formFile.FileName);
                    string[] arr = formFile.FileName.Split(".");
                    string ext = arr[arr.Length - 1];
                    string newName = "questionImage" + q.QuestionId + "." + ext;
                    filePath = "Content/" + newName;
                    q.MediaLink = newName;
                    _questionService.Update(q);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return RedirectToAction("Create", "Exam", new { success = 1 });
        }
    }
}