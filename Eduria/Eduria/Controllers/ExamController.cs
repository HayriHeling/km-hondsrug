using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using System.Web;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Eduria.Controllers
{
    public class ExamController : Controller
    {
        private ExamService _examService;
        private QuestionService _questionService;
        private AnswerService _answerService;
        private ExamQuestionService _examQuestionService;
        private CategoryService _categoryService;

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
        }
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

        public ExamController(ExamService examService, QuestionService questionService, AnswerService answerService, ExamQuestionService examQuestionService, CategoryService categoryService)
        {
            this._examQuestionService = examQuestionService;
            this._examService = examService;
            this._questionService = questionService;
            this._answerService = answerService;
            this._categoryService = categoryService;
        }

        // GET: Exam
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Exam/Details/5
        public IActionResult Show(int id=1)
        {
            //return View(GetExamDataById(id));
            return View(GetExamModelByExamId(id));
        }

        public ExamModel GetExamModelByExamId(int id)
        {
            IEnumerable<ExamQuestion> tempExamQuestions = _examQuestionService.GetAllQuestionIdsAsList(id);
            IEnumerable<Question> tempQuestions = _questionService.GetQuestionsByExamQuestionList(tempExamQuestions);
            IEnumerable<Answer> tempAnswers = _answerService.GetAnswersByQuestionsList(tempQuestions);

            return new ExamModel()
            {
                AnswerModels = CreateAnswerModels(tempAnswers.ToList()),
                Category = 0,
                Description = "",
                ExamId = id,
                Name = "None",
                QuestionModels = CreateQuestionModelsList(tempQuestions.ToList())
            };
        }

        public ExamModel GetExamDataById(int id)
        {
            List<Question> allQuestions = _questionService.GetAll().ToList();
            List<Answer> allAnswers = _answerService.GetAll().ToList();

            List<AnswerModel> answerModels = CreateAnswerModels(allAnswers);
            List<QuestionModel> questionModels = CreateQuestionModelsList(allQuestions);

            return new ExamModel()
            {
                AnswerModels = answerModels,
                Category = 0,
                Description = "",
                ExamId = 0,
                Name = "None",
                QuestionModels = questionModels
            };
        }

        public List<QuestionModel> CreateQuestionModelsList(List<Question> questions)
        {
            List<QuestionModel> outputList = new List<QuestionModel>();
            foreach (Question question in questions)
            {
                outputList.Add(new QuestionModel()
                {
                    Category = question.CategoryId,
                    MediaLink = question.MediaLink,
                    MediaType = 0,
                    QuestionId = question.QuestionId,
                    Text = question.Text
                });
            }

            return outputList;
        }

        public List<AnswerModel> CreateAnswerModels(List<Answer> answers)
        {
            List<AnswerModel> tempAnswerModels = new List<AnswerModel>();
            foreach (Answer answer in answers)
            {
                tempAnswerModels.Add(new AnswerModel()
                {
                    AnswerId = answer.AnswerId,
                    CorrectAnswer = answer.Correct.Equals(0),
                    QuestionId = answer.QuestionId,
                    Text = answer.Text
                });
            }

            return tempAnswerModels;
        }

        //GET: Exam/Create
        public ActionResult Create(int success = 0)
        {
            ViewBag.Success = success;
            ViewBag.questions = _questionService.GetAll();
            ViewBag.categories = _categoryService.GetAll();
            return View();
        }

        public ActionResult CreateExam(string examJson)
        {
            string json = examJson;
            exam exam;
            Debug.WriteLine("---------------> " + examJson);
            try
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    // Deserialization from JSON  
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(exam));
                    exam = (exam)deserializer.ReadObject(ms);
                    Debug.WriteLine("---------------> " + exam.questions.Length);
                }

                Exam ex = new Exam()
                {
                    CategoryId = exam.category,
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
                            CategoryId = q.category,
                            MediaType = q.mediaType,
                            MediaLink = q.mediaLink
                        };
                        _questionService.Add(question);
                        _questionId = _questionService.GetByText(question.Text).QuestionId;
                    }      
                    else
                    {
                        _questionId = _questionService.GetByText(q.text).QuestionId;
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
                Debug.WriteLine("---------------> " + e);
                return RedirectToAction("Create", "Exam");
            }
            return RedirectToAction("Create", "Exam");
        }

        public ActionResult UploadData(IFormFile data)
        {
            try
            {
                Debug.WriteLine("----------------> method is called");
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    data.CopyTo(stream);
                }
                return RedirectToAction("Create", "Exam");
            }
            catch(Exception e)
            {
                Debug.WriteLine("---------------->" + e);
                return RedirectToAction("Create", "Exam");
            }

        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var filePath = "lol";

            foreach(var formFile in files)
            {
                if(formFile.Length > 0)
                {
                    Question q = _questionService.GetByMedia(formFile.FileName);
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



            //for (int i = 0; i <Request.Files.Count; i++)
            //{
            //    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
            //                                                //Use the following properties to get file's name, size and MIMEType
            //    int fileSize = file.ContentLength;
            //    string fileName = file.FileName;
            //    string mimeType = file.ContentType;
            //    System.IO.Stream fileContent = file.InputStream;
            //    //To save file, use SaveAs method
            //    file.SaveAs(Server.MapPath("~/") + fileName); //File will be saved in application root
            //}
            //return Ok(new { count = files.Count, size, filePath });
        }

        //GET: Exam/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //POST: Exam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                //return RedirectToAction(nameof(Index));
                return null;
            }
            catch
            {
                return View();
            }
        }

        //GET: Exam/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //POST: Exam/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                //return RedirectToAction(nameof(Index));
                return null;
            }
            catch
            {
                return View();
            }
        }
    }
}