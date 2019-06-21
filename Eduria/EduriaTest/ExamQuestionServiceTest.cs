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
    public class ExamQuestionServiceTest
    {
        private DbContextOptions<EduriaContext> Options;

        public ExamQuestionServiceTest()
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

        [Fact]
        public void GetAllTest()
        {
            //Arrange 
            var examQuestionList = new List<ExamQuestion>
            {
                new ExamQuestion { ExamId = 1, QuestionId = 1, ExamHasQuestionId = 1 },
                new ExamQuestion { ExamId = 1, QuestionId = 2, ExamHasQuestionId = 1 },
                new ExamQuestion { ExamId = 1, QuestionId = 3, ExamHasQuestionId = 1 },
            }.AsQueryable();

            var examQuestionMockSet = CreateDbSetMock(examQuestionList);

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.ExamQuestions).Returns(examQuestionMockSet.Object);

            var service = new ExamQuestionService(contextMock.Object);
            var examQuestions = service.GetAll();

            //Assert
            Assert.Equal(examQuestionList.Count(), examQuestions.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arrange
            ExamQuestion examQuestion = new ExamQuestion
            {
                ExamId = 1,
                QuestionId = 1,
                ExamHasQuestionId = 1
            };

            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.ExamQuestions.Find(1)).Returns(examQuestion);

            var service = new ExamQuestionService(contextMock.Object);
            ExamQuestion examQuestionById = service.GetById(1);

            //Assert
            Assert.Equal(examQuestion.ExamId, examQuestionById.ExamId);
            Assert.Equal(examQuestion.QuestionId, examQuestionById.QuestionId);
            Assert.Equal(examQuestion.ExamHasQuestionId, examQuestionById.ExamHasQuestionId);
        }

        [Fact]
        public void GetAllQuestionIdsAsList()
        {
            //Arrange
            List<ExamQuestion> examQuestions = new List<ExamQuestion>
            {
                new ExamQuestion { ExamId = 1, QuestionId = 1, ExamHasQuestionId = 1 },
                new ExamQuestion { ExamId = 2, QuestionId = 1, ExamHasQuestionId = 1 },
                new ExamQuestion { ExamId = 2, QuestionId = 2, ExamHasQuestionId = 1 },
                new ExamQuestion { ExamId = 3, QuestionId = 1, ExamHasQuestionId = 1 }
            };

            var examQuestionMockSet = CreateDbSetMock(examQuestions);

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.ExamQuestions).Returns(examQuestionMockSet.Object);

            var service = new ExamQuestionService(contextMock.Object);
            var examQuestionList = service.GetAllQuestionIdsAsList(2);

            //Assert
            Assert.Equal(2, examQuestionList.Count());
        }

        [Fact]
        public void GetExamQuestionByQuestionIdExamIdTest()
        {
            //Arrange
            List<ExamQuestion> examQuestions = new List<ExamQuestion>
            {
                new ExamQuestion { ExamId = 1, QuestionId = 1, ExamHasQuestionId = 1 },
                new ExamQuestion { ExamId = 2, QuestionId = 1, ExamHasQuestionId = 6 },
                new ExamQuestion { ExamId = 2, QuestionId = 2, ExamHasQuestionId = 1 }
            };

            var examQuestionMockSet = CreateDbSetMock(examQuestions);

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.ExamQuestions).Returns(examQuestionMockSet.Object);

            var service = new ExamQuestionService(contextMock.Object);
            ExamQuestion examQuestion = service.GetExamQuestionByQuestionIdExamId(1, 2);

            //Assert
            Assert.Equal(6, examQuestion.ExamHasQuestionId);
        }
    }
}
