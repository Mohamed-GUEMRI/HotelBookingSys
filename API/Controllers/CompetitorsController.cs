using Application.Entities;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitorsController : ControllerBase
    {
        private readonly MockDataService _mockDataService;
        private readonly IHttpClientFactory _httpClientFactory;

        public CompetitorsController(MockDataService mockDataService, IHttpClientFactory httpClientFactory)
        {
            _mockDataService = mockDataService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("prices")]
        public IActionResult GetCompetitorPrices([FromQuery] string season, [FromQuery] string roomType, [FromBody] string[] competitors)
        {
            return Ok(_mockDataService.LoadMockData(competitors,  season,  roomType));
        }
    }
}
