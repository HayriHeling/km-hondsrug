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
    public class ExamServiceTest
    {
        private DbContextOptions<EduriaContext> Options;

        public ExamServiceTest()
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
        /// Create a List of Exams
        /// </summary>
        /// <returns>A list of Exams</returns>
        public static List<Exam> CreateExams()
        {
            List<Exam> examList = new List<Exam>
            {
                new Exam
                {
                    ExamId = 1,
                    Description = "Beschrijving van het eerste examen",
                    Name = "Romeinen",
                    TimeTableId = 1
                },
                new Exam
                {
                    ExamId = 2,
                    Description = "Beschrijving van het tweede examen",
                    Name = "Tweede wereldoorlog",
                    TimeTableId = 5
                },
                new Exam
                {
                    ExamId = 3,
                    Description = "Beschrijving van het derde examen",
                    Name = "Jagers",
                    TimeTableId = 3
                },
                new Exam
                {
                    ExamId = 4,
                    Description = "Beschrijving van het vierde examen",
                    Name = "Boeren",
                    TimeTableId = 7
                },
            };

            return examList;
        }

        [Fact]
        public void GetAllTest()
        {
            //Arrange 
            var examMockSet = CreateDbSetMock(CreateExams());

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Exams).Returns(examMockSet.Object);

            var service = new ExamService(contextMock.Object);
            var exams = service.GetAll();

            //Assert
            Assert.Equal(CreateExams().Count(), exams.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arrange 
            var examMockSet = CreateDbSetMock(CreateExams());

            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Exams).Returns(examMockSet.Object);

            var service = new ExamService(contextMock.Object);
            Exam exam = service.GetById(2);

            //Assert
            Assert.NotNull(exam);
            Assert.Equal(CreateExams()[1].Description, exam.Description);
            Assert.Equal(CreateExams()[1].Name, exam.Name);
            Assert.Equal(CreateExams()[1].TimeTableId, exam.TimeTableId);
        }
        [Fact]
        public void GetByNameTest()
        {
            //Arrange 
            var examMockSet = CreateDbSetMock(CreateExams());

            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Exams).Returns(examMockSet.Object);

            var service = new ExamService(contextMock.Object);
            Exam exam = service.GetByName("Boeren");

            //Assert
            Assert.NotNull(exam);
            Assert.Equal(CreateExams()[3].ExamId, exam.ExamId);
            Assert.Equal(CreateExams()[3].Description, exam.Description);
            Assert.Equal(CreateExams()[3].TimeTableId, exam.TimeTableId);
        }
    }
}
