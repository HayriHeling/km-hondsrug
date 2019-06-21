using Eduria;
using Eduria.Services;
using EduriaData.Models.ExamLayer;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EduriaTest
{
    public class QuestionServiceTest
    {
        private DbContextOptions<EduriaContext> Options;

        public QuestionServiceTest()
        {
            Options = new DbContextOptionsBuilder<EduriaContext>().
                UseInMemoryDatabase(databaseName: "Eduria_Development").
                Options;
        }

        /// <summary>
        /// Method for Mocking generic DbSets.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="elements">IEnumberable filled with the object(s).</param>
        /// <returns>A Mock of the specific DbSet.</returns>
        public static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }

        /// <summary>
        /// Create a List of ExamResults
        /// </summary>
        /// <returns>A list of ExamResults</returns>
        public static List<Question> CreateQuestionList()
        {
            List<Question> questionList = new List<Question>
            {
                new Question
                {
                    QuestionId = 1,
                    QuestionType = (int)QuestionType.Meerkeuze,
                    Text = "Dit is vraag 1",
                    MediaSourceId = 1,
                    TimeTableId = 1
                },
                 new Question
                {
                    QuestionId = 2,
                    QuestionType = (int)QuestionType.Open,
                    Text = "Dit is vraag 2",
                    MediaSourceId = 2,
                    TimeTableId = 2
                },
                new Question
                {
                    QuestionId = 3,
                    QuestionType = (int)QuestionType.Tijdvak,
                    Text = "Dit is vraag 3",
                    MediaSourceId = 3,
                    TimeTableId = 3
                }
            };

            return questionList;
        }

        [Fact]
        public void GetAllTest()
        {
            //Arrange 
            var questonListMockSet = CreateDbSetMock(CreateQuestionList());

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Questions).Returns(questonListMockSet.Object);

            var service = new QuestionService(contextMock.Object);
            var examResults = service.GetAll();

            //Assert
            Assert.Equal(CreateQuestionList().Count(), examResults.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arrange 
            var questionListMockSet = CreateDbSetMock(CreateQuestionList());

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Questions).Returns(questionListMockSet.Object);

            var service = new QuestionService(contextMock.Object);
            Question question = service.GetById(1);

            //Assert
            Assert.NotNull(question);
            Assert.Equal(CreateQuestionList()[0].Text, question.Text);
        }
        
        [Fact]
        public void GetQuestionsByExamQuestionListTest()
        {
            //Arrange 
            var questionListMockSet = CreateDbSetMock(CreateQuestionList());
            var examQuestionList = new List<ExamQuestion>(){
                new ExamQuestion
                {
                    ExamId = 1,
                    QuestionId = 1,
                    ExamHasQuestionId = 1
                },
                new ExamQuestion
                {
                    ExamId = 1,
                    QuestionId = 2,
                    ExamHasQuestionId = 2
                },
                new ExamQuestion
                {
                    ExamId = 1,
                    QuestionId = 4,
                    ExamHasQuestionId = 4
                }
            };

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Questions).Returns(questionListMockSet.Object);

            var service = new QuestionService(contextMock.Object);
            var questions = service.GetQuestionsByExamQuestionList(examQuestionList);

            //Assert
            Assert.NotNull(questions);
            Assert.Equal(2, questions.Count());
        }

        [Fact]
        public void GetQuestionByTextTest()
        {
            //Arrange 
            var questionListMockSet = CreateDbSetMock(CreateQuestionList());

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Questions).Returns(questionListMockSet.Object);

            var service = new QuestionService(contextMock.Object);
            Question question = service.GetQuestionByText("Dit is vraag 3");

            //Assert
            Assert.NotNull(question);
            Assert.Equal(CreateQuestionList()[2].QuestionId, question.QuestionId);
        }

        [Fact]
        public void GetQuestionByText_IsNull_Test()
        {
            //Arrange 
            var questionListMockSet = CreateDbSetMock(CreateQuestionList());

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Questions).Returns(questionListMockSet.Object);

            var service = new QuestionService(contextMock.Object);
            Question question = service.GetQuestionByText("Dit is vraag 4");

            //Assert
            Assert.Null(question);
        }
    }
}
