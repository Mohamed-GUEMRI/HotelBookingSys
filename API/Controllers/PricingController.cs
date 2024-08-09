using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public PricingController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        [HttpGet("calculate-price/{roomType}")]
        public IActionResult CalculatePrice([FromQuery] string roomType, [FromQuery] string season, [FromQuery] int occupancyRate, [FromQuery] string[] competitorPrices)
        {
            var result = _roomService.CalculatePrice(roomType, season, occupancyRate, competitorPrices);
            return Ok(result);
        }
    }
}
