using Eduria;
using Eduria.Controllers;
using Eduria.Models;
using Eduria.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EduriaTest
{
    public class AnalyticControllerTest
    {
        [Fact]
        public void IndexReturnsViewWithAnalyticDefaultModels()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<EduriaContext>().
                UseInMemoryDatabase(databaseName: "Eduria_Development").
                Options;
            var contextMock = new Mock<EduriaContext>(options);

            List<AnalyticHasDefaultModel> models = new List<AnalyticHasDefaultModel>();
            models.Add(new AnalyticHasDefaultModel
            {
                AnalyticDataId = 1,
                AnalyticDefaultId = 1,
                AnalyticDefaultName = "Tijdvakken",
            });
            models.Add(new AnalyticHasDefaultModel
            {
                AnalyticDataId = 1,
                AnalyticDefaultId = 2,
                AnalyticDefaultName = "Woordstramien"
            });

            var mockAnalyticService = new Mock<AnalyticDefaultService>(contextMock.Object);
            mockAnalyticService.Setup(x => x.GetAllDataByAnalyticDataId(1, 1)).Returns(models);

            var userServiceMock = new Mock<UserService>(contextMock.Object);

            var controller = new AnalyticController(mockAnalyticService.Object, userServiceMock.Object);

            //Act
            var result = controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AnalyticHasDefaultModel>>(viewResult.ViewData.Model);

            Assert.Equal(models.Count, model.Count());
        }

        [Fact]
        public void TestMethod()
        {
            //Arrange 

            //Act

            //Assert
        }

    }
}
