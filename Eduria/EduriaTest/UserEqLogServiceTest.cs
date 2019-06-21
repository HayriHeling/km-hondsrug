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
    public class UserEqLogServiceTest
    {
        private DbContextOptions<EduriaContext> Options;

        public UserEqLogServiceTest()
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

        public static List<UserEQLog> CreateUserEQLogList()
        {
            List<UserEQLog> userEQLogs = new List<UserEQLog>
            {
                new UserEQLog
                {
                    UserEQLogId = 1,
                    ExamResultId = 30
                },
                 new UserEQLog
                {
                    UserEQLogId = 1,
                    ExamResultId = 50
                },
                new UserEQLog
                {
                    UserEQLogId = 1,
                    ExamResultId = 60
                }
            };

            return userEQLogs;
        }

        [Fact]
        public void GetAllTest()
        {
            //Arrange 
            var userEqLogMockSet = CreateDbSetMock(CreateUserEQLogList());

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.UserEQLogs).Returns(userEqLogMockSet.Object);

            var service = new UserEQLogService(contextMock.Object);
            var results = service.GetAll();

            //Assert
            Assert.Equal(CreateUserEQLogList().Count(), results.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arrange 
            var userEqLogMockSet = CreateDbSetMock(CreateUserEQLogList());

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.UserEQLogs).Returns(userEqLogMockSet.Object);

            var service = new UserEQLogService(contextMock.Object);
            UserEQLog userEQLog = service.GetById(1);

            //Assert
            Assert.NotNull(userEQLog);
            Assert.Equal(CreateUserEQLogList()[0].ExamResultId, userEQLog.ExamResultId);
        }
    }
}
