using AutoFixture;
using Eduria;
using Eduria.Controllers;
using Eduria.Services;
using EduriaData.Models;
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

       // [Fact]
       // public void AddDataHasDefaultTest()
       //{
       //     //Arrange
       //     var options = new DbContextOptionsBuilder<EduriaContext>().
       //         UseInMemoryDatabase(databaseName: "Eduria_Development").
       //         Options;
       //     var contextMock = new Mock<EduriaContext>(options);

       //     //Act
       //     var service = new AnalyticDefaultService(contextMock.Object);
       //     var result = service.AddDataHasDefault(1, 1);

       //     DataHasDefault dataHasDefault = new DataHasDefault
       //     {
       //         AnalyticDataId = 1,
       //         AnalyticDefaultId = 1
       //     };

       //     //Assert
       //     Assert.Equal(dataHasDefault.AnalyticDataId, result.AnalyticDataId);
       //     Assert.Equal(dataHasDefault.AnalyticDefaultId, result.AnalyticDefaultId);
       // }

        [Fact]
        public void GetAllAnalyticDefaultTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<EduriaContext>().
                UseInMemoryDatabase(databaseName: "Eduria_Development").
                Options;

            var fixture = new Fixture();
            var analyticDefaults = new List<AnalyticDefault>
            {
                fixture.Build<AnalyticDefault>().With(x => x.AnalyticDefaultName, "Kennisbegrippen").Create(),
                fixture.Build<AnalyticDefault>().With(x => x.AnalyticDefaultName, "Woordstramien").Create(),
                fixture.Build<AnalyticDefault>().With(x => x.AnalyticDefaultName, "Tijdvakken").Create()
            }.AsQueryable();

            var analyticDefaultMock = CreateDbSetMock(analyticDefaults);
            var contextMock = new Mock<EduriaContext>(options);
            contextMock.Setup(x => x.AnalyticDefaults).Returns(analyticDefaultMock.Object);

            //Act
            var service = new AnalyticDefaultService(contextMock.Object);
            var result = service.GetAllAnalyticDefault();

            //Assert
            Assert.Equal(analyticDefaults.Count(), result.Count());
        }

        [Fact]
        public void GetAllAnalyticDataTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<EduriaContext>().
                UseInMemoryDatabase(databaseName: "Eduria_Development").
                Options;

            var fixture = new Fixture();          
            var analyticDatas = new List<AnalyticData>
            {
                fixture.Build<AnalyticData>().With(x => x.AnalyticDataId, 1).Create(),
                fixture.Build<AnalyticData>().With(x => x.AnalyticDataId, 2).Create()
            }.AsQueryable();

            var analyticDataMock = CreateDbSetMock(analyticDatas);
            var contextMock = new Mock<EduriaContext>(options);
            contextMock.Setup(x => x.AnalyticDatas).Returns(analyticDataMock.Object);

            //Act
            var service = new AnalyticDefaultService(contextMock.Object);
            var result = service.GetAll();

            //Assert
            Assert.Equal(analyticDatas.Count(), result.Count());
        }

        [Fact]
        public void GetAllDataHasDefaultTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<EduriaContext>().
                UseInMemoryDatabase(databaseName: "Eduria_Development").
                Options;

            var fixture = new Fixture();
            var dataHasDefaults = new List<DataHasDefault>
            {
                fixture.Build<DataHasDefault>().With(x => x.DataHasDefaultId, 1).Create(),
                fixture.Build<DataHasDefault>().With(x => x.DataHasDefaultId, 2).Create(),
                fixture.Build<DataHasDefault>().With(x => x.DataHasDefaultId, 3).Create(),
                fixture.Build<DataHasDefault>().With(x => x.DataHasDefaultId, 4).Create()
            }.AsQueryable();

            var analyticDataHasDefaultMock = CreateDbSetMock(dataHasDefaults);
            var contextMock = new Mock<EduriaContext>(options);
            contextMock.Setup(x => x.DataHasDefaults).Returns(analyticDataHasDefaultMock.Object);

            //Act
            var service = new AnalyticDefaultService(contextMock.Object);
            var result = service.GetAllDataHasDefaults();

            //Assert
            Assert.Equal(dataHasDefaults.Count(), result.Count());
        }
    }
}
