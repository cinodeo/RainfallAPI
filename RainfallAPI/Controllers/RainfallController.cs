// Controller Layer
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RainfallAPI.Controllers
{
    [ApiController]
    [Route("rainfall")]
    public class RainfallController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiUrl;


        public RainfallController(IHttpClientFactory clientFactory, string apiUrl)
        {
            _clientFactory = clientFactory;
            _apiUrl = apiUrl;

        }
        /// <summary>
        /// Gets rainfall readings for a specific station.
        /// </summary>
        /// <param name="stationId">The ID of the station.</param>
        /// <param name="count">The number of readings to return.</param>
        /// <response code="200">A list of rainfall readings successfully retrieved</response>
        /// <response code="400">Invalid request.</response>
        /// <response code="404">No readings found for the specified stationId</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("id/{stationId}/readings")]
        [ProducesResponseType(typeof(RainfallReadingResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, [FromQuery] int count = 10)
        {
            var client = _clientFactory.CreateClient();
            var apiUrl = $"{_apiUrl}/id/stations/{stationId}/readings?_sorted&_limit={count}";

            var response = await client.GetAsync(apiUrl);

            if (string.IsNullOrEmpty(stationId))
            {
                return BadRequest("Station ID cannot be empty");
            }
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var rainfallReadingResponse = JsonConvert.DeserializeObject<RainfallReadingResponse>(responseData);
                var jsonObject = JObject.Parse(responseData);
                var items = jsonObject["items"];

                if (items == null || !items.HasValues)
                {
                    // "items" array is empty, return 404 Not Found
                    return NotFound("No readings found for the specified stationId");
                }
                else 
                {
                    return Ok(new { RainfallReadings = rainfallReadingResponse });
                }
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
