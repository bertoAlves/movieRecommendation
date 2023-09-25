using APIGateway.Controllers;
using APIGateway.Services.Interfaces;
using Common.AVProductMS.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Test.Controllers
{
    public class RecommendationControllerTest
    {
        [Fact]
        public async Task GetAllTimeMoviesRecommendation_EmptyCriteria_ShouldThrowBadRequest()
        {
            var recommendationServiceMock = new Mock<IRecommendationService>();

            var controller = new RecommendationController(recommendationServiceMock.Object);

            var result = await controller.GetAllTimeMoviesRecommendation(keywords: null, genres: null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("At least one search criteria must be specified", badRequestResult.Value);
        }

        [Fact]
        public async Task GetIntelligentBillboard_ValidParameters_ReturnsOk()
        {
            var recommendationServiceMock = new Mock<IRecommendationService>();
            var expectedBillboard = new IntelligentBillboard();

            recommendationServiceMock.Setup(service =>
                service.GetIntelligentBillboard(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(expectedBillboard);

            var controller = new RecommendationController(recommendationServiceMock.Object);

            var result = await controller.GetIntelligentBillboard(numberWeeks: 4, bigScreens: 2, smallScreens: 3, useSuccessfulGenres: true);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(expectedBillboard, okResult.Value);
        }


        [Fact]
        public async Task GetIntelligentBillboard_ZeroNumberOfWeeks_ShouldThrowBadRequest()
        {
            var recommendationServiceMock = new Mock<IRecommendationService>();
            var expectedBillboard = new IntelligentBillboard();

            recommendationServiceMock.Setup(service =>
                service.GetIntelligentBillboard(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(expectedBillboard);

            var controller = new RecommendationController(recommendationServiceMock.Object);

            var result = await controller.GetIntelligentBillboard(numberWeeks: 0, bigScreens: 2, smallScreens: 3, useSuccessfulGenres: true);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The 'numberOfWeeks' parameter must be a positive number.", badRequest.Value);
        }


        [Fact]
        public async Task GetIntelligentBillboard_ZeroNumberOfBigScreensAndSmallScreens_ShouldThrowBadRequest()
        {
            var recommendationServiceMock = new Mock<IRecommendationService>();
            var expectedBillboard = new IntelligentBillboard();

            recommendationServiceMock.Setup(service =>
                service.GetIntelligentBillboard(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(expectedBillboard);

            var controller = new RecommendationController(recommendationServiceMock.Object);

            var result = await controller.GetIntelligentBillboard(numberWeeks: 1, bigScreens: 0, smallScreens: 0, useSuccessfulGenres: true);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Either 'bigScreens' or 'smallScreens' must be a positive number.", badRequest.Value);
        }

    }
}
