using AutoFixture;
using Eduria;
using Eduria.Controllers;
using Eduria.Services;
using EduriaData.Models.AnalyticLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EduriaTest
{
    public class AnalyticServiceTest
    {
        [Fact]
        public void AddDataHasDefaultTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<EduriaContext>().
                UseInMemoryDatabase(databaseName: "Eduria_Development").
                Options;
            var contextMock = new Mock<EduriaContext>(options);

            //Act
            var service = new AnalyticDefaultService(contextMock.Object);
            var result = service.AddDataHasDefault(1, 1);

            DataHasDefault dataHasDefault = new DataHasDefault
            {
                AnalyticDataId = 1,
                AnalyticDefaultId = 1
            };

            //Assert
            Assert.Equal(dataHasDefault.AnalyticDataId, result.AnalyticDataId);
            Assert.Equal(dataHasDefault.AnalyticDefaultId, result.AnalyticDefaultId);
        }

        [Fact]
        public void GetAllAnalyticDefaultTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<EduriaContext>().
                UseInMemoryDatabase(databaseName: "Eduria_Development").
                Options;

            var fixture = new Fixture();
            var analyticDefault = fixture.Build<AnalyticDefault>().With(x => x.AnalyticDefaultName, "Kennisbegrippen").Create();

            var analyticDefaults = new List<AnalyticDefault>
            {
                fixture.Build<AnalyticDefault>().With(x => x.AnalyticDefaultName, "Kennisbegrippen").Create(),
                fixture.Build<AnalyticDefault>().With(x => x.AnalyticDefaultName, "Woordstramien").Create(),
                fixture.Build<AnalyticDefault>().With(x => x.AnalyticDefaultName, "Tijdvakken").Create()
            }.AsQueryable();

            var analyticDefaultMock = new Mock<DbSet<AnalyticDefault>>();
            analyticDefaultMock.As<IQueryable<AnalyticDefault>>().Setup(x => x.Provider).Returns(analyticDefaults.Provider);
            analyticDefaultMock.As<IQueryable<AnalyticDefault>>().Setup(x => x.Expression).Returns(analyticDefaults.Expression);
            analyticDefaultMock.As<IQueryable<AnalyticDefault>>().Setup(x => x.ElementType).Returns(analyticDefaults.ElementType);
            analyticDefaultMock.As<IQueryable<AnalyticDefault>>().Setup(x => x.GetEnumerator()).Returns(analyticDefaults.GetEnumerator());

            var contextMock = new Mock<EduriaContext>(options);
            contextMock.Setup(x => x.AnalyticDefaults).Returns(analyticDefaultMock.Object);

            //Act
            var service = new AnalyticDefaultService(contextMock.Object);
            var result = service.GetAllAnalyticDefault();

            //Assert
            Assert.Equal(analyticDefaults.Count(), result.Count());
        }
    }
}
