using System;
using System.Collections.Generic;
using System.Linq;
using Eduria.JsonClasses;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using EduriaData.Models.ExamLayer;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Eduria.Controllers
{
    public class ExamController : Controller
    {
        private ExamService ExamService;
        private QuestionService QuestionService;
        private AnswerService AnswerService;
        private TimeTableService TimeTableService;
        private ExamQuestionService ExamQuestionService;
        private MediaSourceService MediaSourceService;
        private UserEQLogService UserEqLogService;
        private ExamResultService ExamResultService;
        private UserService UserService;

        /// <summary>
        /// internal class for databinding the information from database to object.
        /// </summary>
        [DataContract]
        class exam
        {
            [DataMember]
            public int id;
            [DataMember]
            public string name;
            [DataMember]
            public string description;
            [DataMember]
            public int category;
            [DataMember]
            public question[] questions;
            [DataMember]
            public string dateStarted;
            [DataMember]
            public string dateEnded;
            [DataMember]
            public int score;
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
            [DataMember]
            public string chosenAnswer;
            [DataMember]
            public int correctAnswered = 0;
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
            UserEQLogService userEqLogService, ExamResultService examResultService, MediaSourceService mediaSourceService, UserService userService)
        {
            ExamQuestionService = examQuestionService;
            ExamService = examService;
            QuestionService = questionService;
            TimeTableService = timeTableService;
            UserEqLogService = userEqLogService;
            ExamResultService = examResultService;
            AnswerService = answerService;
            MediaSourceService = mediaSourceService;
            UserService = userService;
        }

        /// <summary>
        /// Method that creates a view for an exammodel.
        /// </summary>
        /// <param name="id">Id of the exammodel</param>
        /// <param name="examResult"></param>
        /// <returns>View of the exammodel</returns>
        public IActionResult Show(int id = 1, int examResult = -1)
        {
            //return View(GetExamDataById(id));
            _examId = id;
            _dateTime = DateTime.Now;
            if (examResult < 0)
            {
                return View(GetExamModelByExamId(id));
            }
            return View(ResumeExamModel(examResult));
        }
        /// <summary>
        /// Get the exam of given id and changes database data into models to use in the view. 
        /// </summary>
        /// <param name="id">The id of the exam from the databases</param>
        /// <returns></returns>
        public IActionResult Take(int id)
        {
            ViewBag.exam = ExamService.GetById(id);
            IEnumerable<ExamQuestion> data = ExamQuestionService.GetAllQuestionIdsAsList(id);
            IEnumerable<Question> dataQuestions = QuestionService.GetQuestionsByExamQuestionList(data);
            IEnumerable<Answer> dataAnswers = AnswerService.GetAnswersByQuestionsList(dataQuestions);
            IEnumerable<TimeTable> dataTimeTables = TimeTableService.GetAll();
            List<QuestionModel> questions = new List<QuestionModel>();
            List<AnswerModel> answers = new List<AnswerModel>();
            List<TimeTableModel> timetables = new List<TimeTableModel>();
            foreach(TimeTable timeTable in dataTimeTables)
            {
                TimeTable t = timeTable;
                TimeTableModel tModel = new TimeTableModel()
                {
                    TimeTableId = t.TimeTableId,
                    Text = t.Text,
                    MediaSourceModel = ConvertToMediaSourceModel(MediaSourceService.GetById(t.MediaSourceId))
                };
                timetables.Add(tModel);
            }
            foreach(Question examQuestion in dataQuestions)
            {
                Question q = examQuestion;
                QuestionModel qModel = new QuestionModel()
                {
                    QuestionId = q.QuestionId,
                    QuestionType = q.QuestionType,
                    Text = q.Text,
                    MediaSourceModel = ConvertToMediaSourceModel(MediaSourceService.GetById(q.MediaSourceId)),
                    TimeTableModel = ConvertToTimeTableModel(TimeTableService.GetById(q.TimeTableId))
                }; 
                questions.Add(qModel);
            }
            foreach(Answer examAnswer in dataAnswers)
            {
                Answer a = examAnswer;
                AnswerModel aModel = new AnswerModel()
                {
                    AnswerId = a.AnswerId,
                    Text = a.Text,
                    QuestionId = a.QuestionId,
                    CorrectAnswer = (a.Correct == 1)
                };
                answers.Add(aModel);
            }
            ViewBag.questions = questions;
            ViewBag.answers = answers;
            ViewBag.timetables = timetables;
            return View();
        }
        /// <summary>
        /// Calculates the results of the taken exam and calculates which answers were right and which answers were given.
        /// Returns the exammodel and score in viewbag.
        /// </summary>
        /// <param name="examJson">The Json of the taken exam</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Results(string examJson)
        {
            ExamModel examModel;
            exam exam;
            List<QuestionAnswerModel> matches = new List<QuestionAnswerModel>();
            try
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(examJson)))
                {
                    // Deserialization from JSON  
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(exam));
                    exam = (exam)deserializer.ReadObject(ms);
                }

                ExamResult examResult = new ExamResult()
                {
                    ExamId = exam.id,
                    UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    StartedAt = Convert.ToDateTime(exam.dateStarted),
                    FinishedAt = Convert.ToDateTime(exam.dateEnded)
                };
                ExamResultService.Add(examResult);
                int resultId = ExamResultService.GetExamResultByUserAndStartDate(examResult.UserId, examResult.StartedAt).ExamResultId;
                int questionAmount = exam.questions.Length;
                int correctAmount = 0;
                foreach (question q in exam.questions)
                {
                    int CorrectId = 0;
                    string CorrectText = "";
                    int AnsweredId = 0;
                    string AnsweredText = "";
                    switch ((QuestionType)q.questionType)
                    {
                        case QuestionType.Meerkeuze:
                            foreach (answer a in q.answers)
                            {
                                a.correct = AnswerService.GetById(a.id).Correct;
                                if (a.correct == 1)
                                {
                                    CorrectId = a.id;
                                    CorrectText = a.text;
                                    if (q.chosenAnswer == a.id.ToString())
                                    {
                                        correctAmount++;
                                        q.correctAnswered = 1;
                                    }
                                }
                                if(q.chosenAnswer == a.id.ToString())
                                {
                                    AnsweredId = a.id;
                                    AnsweredText = a.text;
                                }
                            }
                            break;
                        case QuestionType.Tijdvak:
                            if(q.chosenAnswer == q.category.ToString())
                            {
                                correctAmount++;
                                q.correctAnswered = 1;
                            }
                            CorrectId = q.category;
                            CorrectText = TimeTableService.GetById(q.category).Text;
                            if(q.chosenAnswer != null)
                            {                                
                                AnsweredId = int.Parse(q.chosenAnswer);
                                AnsweredText = TimeTableService.GetById(AnsweredId).Text;
                            }
                            else
                            {
                                AnsweredId = 0;
                                AnsweredText = "none";
                            }

                            
                            break;
                        case QuestionType.Open:
                            if(q.chosenAnswer.ToLower() == q.answers[0].text.ToLower())
                            {
                                correctAmount++;
                                q.correctAnswered = 1;
                            }
                            CorrectId = q.answers[0].id;
                            CorrectText = q.answers[0].text;
                            AnsweredId = 0;
                            AnsweredText = q.chosenAnswer.ToLower();
                            break;
                    }
                    matches.Add(new QuestionAnswerModel()
                    {
                        QuestionId = q.id,
                        QuestionText = q.text,
                        AnswerId = CorrectId,
                        AnswerText = CorrectText,
                        GivenAnswerId = AnsweredId,
                        GivenAnswerText = AnsweredText
                    });
                    UserEQLogModel userEqLogModel = new UserEQLogModel
                    {
                        ExamHasQuestionId = ExamQuestionService.GetExamQuestionByQuestionIdExamId(q.id, exam.id).ExamHasQuestionId,
                        ExamResultId = resultId,
                        UserId = examResult.UserId,
                        TimesWrong = 0,
                        AnsweredOn = Convert.ToDateTime(exam.dateEnded),
                        CorrectAnswered = q.correctAnswered
                    };
                    AddOrUpdateUserEqLog(userEqLogModel, q.id, exam.id, resultId);

                }
                examResult.Score = ((100/questionAmount) * correctAmount);
                ExamResultService.Update(examResult);               
                examModel = new ExamModel()
                {
                    ExamId = exam.id,
                    Name = ExamService.GetById(exam.id).Name,
                    Description = ExamService.GetById(exam.id).Description
                };
                ViewBag.exam = examModel;
                ViewBag.score = (examResult.Score + 1);
                ViewBag.questions = matches;
                return View();
            }
            catch (Exception e)
            {
                return Content(e.ToString());
                throw e;
            }
        }
        /// <summary>
        /// Shows an overview of all exams.
        /// </summary>
        /// <returns></returns>
        public IActionResult OverView()
        {
            ViewBag.userType = UserService.GetById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)).UserType;
            ViewBag.exams = ExamService.GetAll();
            ViewBag.ttService = TimeTableService;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public void ToggleActiveExam(int examId, int state)
        {
            Exam exam = ExamService.GetById(examId);
            exam.IsActive = state;
            ExamService.Update(exam);
        }

        /// <summary>
        /// Method used for sending the data from the exam to this controller
        /// </summary>
        /// <param name="jsoninput">Json data</param>
        /// <param name="examId"></param>
        /// <param name="userId"></param>
        /// <param name="score"></param>
        /// <param name="examResultId"></param>
        /// <returns></returns>
        public void SendResults(string jsoninput, string examId, string userId, string score, string starttime, string endtime, string examResultId)
        {
            int examResultIdOutput = 0;
            int examIdInt = Int32.Parse(examId);
            int userIdInt = Int32.Parse(userId);
            int scoreInt = Int32.Parse(score);
            DateTime startTime = ConvertToDateTime(starttime);
            DateTime endTime = ConvertToDateTime(endtime);
            int examResultIdInt = Int32.Parse(examResultId);
            if (examResultIdInt < 0)
            {
                ImportExamResultToDatabase(examIdInt, userIdInt, scoreInt, startTime, endTime);
                examResultIdOutput = ExamResultService.GetExamResultByUserAndStartDate(userIdInt, startTime).ExamResultId;
            }
            else
            {
                ExamResult examResult = ExamResultService.GetById(examResultIdInt);
                examResult.FinishedAt = endTime;
                examResult.Score = scoreInt;
                ExamResultService.Update(examResult);
                examResultIdOutput = examResultIdInt;
            }
            
            ImportQuestionsToDatabase(CreatEqLogJsonsFromJson(jsoninput), examIdInt, examResultIdOutput, userIdInt);
            
        }

        /// <summary>
        /// Method to convert an javascript to json datetime object to a .net DateTime object
        /// </summary>
        /// <param name="jsonDate"></param>
        /// <returns></returns>
        private DateTime ConvertToDateTime(string jsonDate)
        {
            DateTime outputDateTime;
            if (jsonDate == "null")
            {
                outputDateTime = DateTime.MinValue;
            }
            else
            {
                jsonDate = jsonDate.Replace("T", " ");
                jsonDate = jsonDate.Replace("Z", " ");
                jsonDate = jsonDate.Replace("\"", "");
                outputDateTime = DateTime.Parse(jsonDate);
            }

            return outputDateTime;
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
        /// <param name="examResultId"></param>
        /// <param name="userId"></param>
        public void ImportQuestionsToDatabase(List<UserEqLogJson> userEqLogJsons, int examId, int examResultId, int userId = 1)
        {
            foreach (UserEqLogJson userEqLogJson in userEqLogJsons)
            {
                UserEQLogModel userEqLogModel = new UserEQLogModel
                {
                    AnsweredOn = userEqLogJson.AnsweredOn, 
                    CorrectAnswered = userEqLogJson.CorrectAnswered,
                    ExamHasQuestionId = ExamQuestionService.GetExamQuestionByQuestionIdExamId(userEqLogJson.QuestionId, examId).ExamHasQuestionId,
                    ExamResultId = examResultId,
                    TimesWrong = userEqLogJson.TimesWrong,
                    UserId = userId
                };
                AddOrUpdateUserEqLog(userEqLogModel, userEqLogJson.QuestionId, examId, examResultId);
            }
        }

        /// <summary>
        /// Method that checks if an UserEQLog entry already exists, and handles according to the answer.
        /// </summary>
        /// <param name="userEqLogModel"></param>
        /// <param name="questionId"></param>
        /// <param name="examId"></param>
        /// <param name="examResultId"></param>
        public void AddOrUpdateUserEqLog(UserEQLogModel userEqLogModel, int questionId, int examId, int examResultId)
        {
            int examQuestionId = ExamQuestionService.GetExamQuestionByQuestionIdExamId(questionId, examId)
                .ExamHasQuestionId;
            UserEQLog userEqLog = UserEqLogService.GetByResultUserQuestionId(examResultId, userEqLogModel.UserId, examQuestionId);
            if (userEqLog != null)
            {
                userEqLog.TimesWrong = userEqLogModel.TimesWrong;
                userEqLog.CorrectAnswered = userEqLogModel.CorrectAnswered;
                UserEqLogService.Update(userEqLog);
            }
            else
            {
                userEqLog = new UserEQLog()
                {
                    AnsweredOn = userEqLogModel.AnsweredOn,
                    CorrectAnswered = userEqLogModel.CorrectAnswered,
                    ExamHasQuestionId = ExamQuestionService.GetExamQuestionByQuestionIdExamId(questionId, examId).ExamHasQuestionId,
                    ExamResultId = examResultId,
                    TimesWrong = userEqLogModel.TimesWrong,
                    UserId = userEqLogModel.UserId
                };
                UserEqLogService.Add(userEqLog);
            }
        }

        /// <summary>
        /// Function that imports the result of the exam in the database.
        /// </summary>
        public void ImportExamResultToDatabase(int examId, int userId, int score, DateTime start, DateTime end)
        {
            ExamResult examResult = new ExamResult()
            {
                ExamId = examId,
                StartedAt = start,
                FinishedAt = end,
                UserId = userId,
                Score = score
            };
            ExamResultService.Add(examResult);
        }

        /// <summary>
        /// Function that returns an ExamModel by the id of the exam.
        /// </summary>
        /// <param name="id">Examid</param>
        /// <returns>An complete ExamModel</returns>
        public ExamModel GetExamModelByExamId(int id)
        {
            IEnumerable<ExamQuestion> tempExamQuestions = ExamQuestionService.GetAllQuestionIdsAsList(id);
            IEnumerable<Question> tempQuestions = QuestionService.GetQuestionsByExamQuestionList(tempExamQuestions);
            Exam exam = ExamService.GetById(id);
            return new ExamModel()
            {
                AnswerModels = null, //CreateAnswerModels(tempAnswers.ToList()),
                TimeTable = ConvertToTimeTableModel(TimeTableService.GetById(exam.TimeTableId)),
                Description = exam.Description,
                ExamId = id,
                Name = exam.Name,
                QuestionModels = CreateQuestionModelsList(tempQuestions.ToList()),
                ExamResultId = -1,
                Score = -1
            };
        }

        /// <summary>
        /// Method that resumes an already partly made exam. It uses an examResultId to get the correct data.
        /// </summary>
        /// <param name="examResultId"></param>
        /// <returns></returns>
        public ExamModel ResumeExamModel(int examResultId)
        {
            ExamResult examResult = ExamResultService.GetById(examResultId);
            IEnumerable<ExamQuestion> examQuestions = ExamQuestionService.GetAllQuestionIdsAsList(examResult.ExamId).ToList();
            IEnumerable<UserEQLog> tempEqLogs = UserEqLogService.GetAllByResultId(examResultId);

            List<ExamQuestion> examQuestionsOutput = examQuestions.ToList();

            foreach (ExamQuestion question in examQuestions)
            {
                foreach (UserEQLog log in tempEqLogs)
                {
                    
                    if (question.ExamHasQuestionId == log.ExamHasQuestionId && log.CorrectAnswered.Equals(1))
                    {
                        examQuestionsOutput.Remove(question);
                    }
                }
            }

            IEnumerable<Question> tempQuestions = QuestionService.GetQuestionsByExamQuestionList(examQuestionsOutput);
            Exam exam = ExamService.GetById(examResult.ExamId);
            return new ExamModel
            {
                AnswerModels = null, //CreateAnswerModels(tempAnswers.ToList()),
                TimeTable = ConvertToTimeTableModel(TimeTableService.GetById(exam.TimeTableId)),
                Description = exam.Description,
                ExamId = examResult.ExamId,
                Name = exam.Name,
                QuestionModels = CreateQuestionModelsList(tempQuestions.ToList()),
                ExamResultId = examResultId, 
                Score = examResult.Score
            };
        }

        /// <summary>
        /// Gets the questionmodels and answermodels which belong to the examId.
        /// </summary>
        /// <param name="id">The examId</param>
        /// <returns>a complete Exammodel</returns>
        public ExamModel GetExamDataById(int id)
        {
            List<Question> allQuestions = QuestionService.GetAll().ToList();
            Exam exam = ExamService.GetById(id);
            List<QuestionModel> questionModels = CreateQuestionModelsList(allQuestions);
            return new ExamModel()
            {
                AnswerModels = null,
                TimeTable = ConvertToTimeTableModel(TimeTableService.GetById(exam.TimeTableId)),
                Description = exam.Description,
                ExamId = id,
                Name = exam.Name,
                QuestionModels = questionModels,
                Score = -1
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
                    TimeTableModel = ConvertToTimeTableModel(TimeTableService.GetById(question.TimeTableId)),
                    MediaSourceModel = ConvertToMediaSourceModel(MediaSourceService.GetById(question.MediaSourceId)),
                    QuestionId = question.QuestionId,
                    Text = question.Text,
                    QuestionType = question.QuestionType
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

            IEnumerable<TimeTable> tables = TimeTableService.GetAll();
            List<TimeTableModel> tableModels = new List<TimeTableModel>();
            foreach(TimeTable table in tables)
            {
                
                tableModels.Add(ConvertToTimeTableModel(table));
            }
            ViewBag.categories = tableModels;

            IEnumerable<Question> questions = QuestionService.GetAll();
            List<QuestionModel> questionModels = new List<QuestionModel>();
            foreach (Question question in questions)
            {
                QuestionModel questionModel = new QuestionModel()
                {
                    QuestionId = question.QuestionId,
                    QuestionType = question.QuestionType,
                    TimeTableModel = tableModels.First(x => x.TimeTableId == question.TimeTableId),
                    Text = question.Text,
                    MediaSourceModel = ConvertToMediaSourceModel(MediaSourceService.GetById(question.MediaSourceId))
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
                ExamService.Add(ex);
                int examId = ExamService.GetByName(ex.Name).ExamId;

                foreach(question q in exam.questions)
                {
                    int QuestionId;
                    if (!q.existing)
                    {
                        int mediaId;
                        if(q.mediaType != 0)
                        {
                            MediaSource src = new MediaSource()
                            {
                                MediaType = q.mediaType,
                                Source = q.mediaLink
                            };
                            MediaSourceService.Add(src);
                            mediaId = MediaSourceService.GetBySource(src.Source).MediaSourceId;
                        }else
                        {
                            mediaId = MediaSourceService.GetByMediaType(0).MediaSourceId;
                        }
                        Question question = new Question()
                        {
                            Text = q.text,
                            QuestionType = q.questionType,
                            TimeTableId = q.category,
                            MediaSourceId = mediaId
                        };
                        QuestionService.Add(question);
                        QuestionId = QuestionService.GetQuestionByText(question.Text).QuestionId;
                    }      
                    else
                    {
                        QuestionId = QuestionService.GetQuestionByText(q.text).QuestionId;
                    }

                    ExamQuestion eq = new ExamQuestion()
                    {
                        ExamId = examId,
                        QuestionId = QuestionId
                    };

                    ExamQuestionService.Add(eq);
                    if (!q.existing)
                    {
                        foreach (answer a in q.answers)
                        {
                            Answer answer = new Answer()
                            {
                                QuestionId = QuestionId,
                                Text = a.text,
                                Correct = a.correct
                            };
                            AnswerService.Add(answer);
                        }
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
                    Question q = QuestionService.GetQuestionByMediaLink(formFile.FileName);
                    string[] arr = formFile.FileName.Split(".");
                    string ext = arr[arr.Length - 1];
                    string newName = "questionMedia" + q.QuestionId + "." + ext;
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Content\\", newName);
                    MediaSource src = MediaSourceService.GetById(q.MediaSourceId);
                    src.Source = newName;
                    MediaSourceService.Update(src);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return RedirectToAction("Create", "Exam", new { success = 1 });
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
                Text = timeTable.Text
            };
        }
    }
}