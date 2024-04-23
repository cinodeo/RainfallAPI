using NUnit.Framework;
using RainfallAPI.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RainfallAPI.Tests
{
    [TestFixture]
    public class RainfallControllerTests
    {
        private RainfallController _controller;

        [SetUp]
        public void Setup()
        {
            var mockFactory = new Mock<IHttpClientFactory>();

            // Create an instance of the controller or mock any dependencies
            _controller = new RainfallController(mockFactory.Object);
        }

        //[Test]
        //public void GetRainfallReadings_ValidStationId_ReturnsData()
        //{
        //    // Arrange
        //    string stationId = "3680";
        //    int count = 10; // Set count to whatever is appropriate for your test

        //    var mockFactory = new Mock<IHttpClientFactory>();
        //    var mockClient = new Mock<HttpClient>();
        //    mockFactory.Setup(x => x.CreateClient()).Returns(mockClient.Object);

        //    var mockResponse = new Mock<HttpResponseMessage>();
        //    mockResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);
        //    // Set up the mock response content as needed for your test

        //    mockClient.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(mockResponse.Object));

        //    _controller = new RainfallController(mockFactory.Object);

        //    // Act
        //    var resultTask = _controller.GetRainfallReadings(stationId, count);

        //    // Assert
        //    // Await the task to get the actual IActionResult
        //    var result = resultTask.Result;

        //    Assert.That(result, resultTask); // Simple assertion to ensure a value is returned
        //                            // You can add further assertions based on the expected IActionResult
        //}




        [Test]
        public async Task GetRainfallReadings_InvalidStationId_ReturnsBadRequest()
        {
            // Arrange
            string stationId = "897788";
            int count = 10; // Set count to whatever is appropriate for your test

            // Act
            var result = await _controller.GetRainfallReadings(stationId, count);

            // Assert
            // Await the task to get the IActionResult result
            Assert.That(result, Is.TypeOf<BadRequestResult>());

            // You can add more specific assertions on the IActionResult properties (e.g., status code)
        }
        //Add another test to  check if items is empty
        //Add another test to check if items are not empty
    }
}
