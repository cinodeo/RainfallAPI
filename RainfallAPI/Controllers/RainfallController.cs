using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RainfallAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RainfallController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public RainfallController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        [HttpGet("id/{stationId}/readings")]
        [ProducesResponseType(typeof(string), 200)] // Document the response type for 200 status code
        [ProducesResponseType(typeof(string), 400)] // Document the response type for 400 status code
        [ProducesResponseType(typeof(string), 404)] // Document the response type for 404 status code
        [ProducesResponseType(typeof(string), 500)] // Document the response type for 500 status code
        public async Task<IActionResult> GetRainfallReadings(string stationId, [FromQuery] int count = 10)
        {
            var client = _clientFactory.CreateClient();
            var apiUrl = $"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings?_sorted&_limit={count}";

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Failed to retrieve rainfall readings");
            }
        }

    }
}
