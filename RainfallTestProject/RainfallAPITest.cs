using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using RainfallAPI.Controllers;


namespace RainfallAPITests
{
    [TestFixture]
    public class RainfallControllerTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<HttpMessageHandler> _handlerMock;
        private RainfallController _controller;

        [SetUp]
        public void Setup()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new System.Uri("https://environment.data.gov.uk/flood-monitoring/")
            };
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _controller = new RainfallController(_httpClientFactoryMock.Object, "https://environment.data.gov.uk/flood-monitoring");
        }

        [Test]
        public async Task GetRainfallReadings_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var stationId = "3680";
            var count = 10;
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"items\": [{\"value\": 10.5, \"dateTime\": \"2023-04-01T12:00:00Z\"}]}")
            };
            _handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetRainfallReadings(stationId, count);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }


        [Test]
        public async Task GetRainfallReadings_ApiReturnsNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var stationId = "station123";
            var count = 10;
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };
            _handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetRainfallReadings(stationId, count);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}