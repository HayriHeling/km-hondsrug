using Eduria;
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
    public class ExamResultServiceTest
    {
        private DbContextOptions<EduriaContext> Options;

        public ExamResultServiceTest()
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
        public static List<ExamResult> CreateExamResults()
        {
            List<ExamResult> examQuestionList = new List<ExamResult>
            {
                new ExamResult
                {
                    ExamId = 1,
                    ExamResultId = 1,
                    UserId = 1,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    Score = 30
                },
                new ExamResult
                {
                    ExamId = 2,
                    ExamResultId = 2,
                    UserId = 1,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    Score = 60
                },
                new ExamResult
                {
                    ExamId = 1,
                    ExamResultId = 1,
                    UserId = 2,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    Score = 40
                },
                new ExamResult
                {
                    ExamId = 1,
                    ExamResultId = 1,
                    UserId = 3,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    Score = 30
                },
            };

            return examQuestionList;
        }

        [Fact]
        public void GetAllTest()
        {
            //Arrange 
            var examResultMockSet = CreateDbSetMock(CreateExamResults());

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.ExamResults).Returns(examResultMockSet.Object);

            var service = new ExamResultService(contextMock.Object);
            var examResults = service.GetAll();

            //Assert
            Assert.Equal(CreateExamResults().Count(), examResults.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arrange 
            var examResultMockSet = CreateDbSetMock(CreateExamResults());

            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.ExamResults).Returns(examResultMockSet.Object);

            var service = new ExamResultService(contextMock.Object);
            ExamResult examResult = service.GetById(2);

            //Assert
            Assert.NotNull(examResult);
            Assert.Equal(2, examResult.ExamId);
            Assert.Equal(2, examResult.ExamResultId);
            Assert.Equal(1, examResult.UserId);
            Assert.IsType<DateTime>(examResult.StartedAt);
            Assert.IsType<DateTime>(examResult.FinishedAt);
            Assert.Equal(60, examResult.Score);
        }

        [Fact]
        public void GetExamResultByUserAndExamId()
        {
            //Arrange 
            var examResultMockSet = CreateDbSetMock(CreateExamResults());

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.ExamResults).Returns(examResultMockSet.Object);

            var service = new ExamResultService(contextMock.Object);
            ExamResult examResult = service.GetExamResultByUserAndExamId(1, 1);

            //Assert
            Assert.Equal(1, examResult.ExamResultId);
            Assert.Equal(30, examResult.Score);
        }
    }  
}
