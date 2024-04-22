// Controller Layer
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RainfallAPI.Models;

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
        [ProducesResponseType(typeof(RainfallReadingResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetRainfallReadings(string stationId, [FromQuery] int count = 100)
        {
            var client = _clientFactory.CreateClient();
            var apiUrl = $"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings?_sorted&_limit={count}";

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var rainfallReadingResponse = JsonConvert.DeserializeObject<RainfallReadingResponse>(responseData);
                return Ok(rainfallReadingResponse);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Failed to retrieve rainfall readings");
            }
        }

    }
}
