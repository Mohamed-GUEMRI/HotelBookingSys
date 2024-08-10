using Application.DTOs;
using Application.Entities;
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

        [HttpGet("calculate-price")]
        public async Task<IActionResult> CalculatePrice([FromQuery] string roomType, [FromQuery] string season, [FromQuery] int occupancyRate, [FromQuery] string[] competitors)
        {
            try
            {
                // Fetch competitor prices
                var competitorPrices = await _roomService.GetCompetitorPrices(season, roomType, competitors);

                if (competitorPrices == null || competitorPrices.Count == 0)
                {
                    return NotFound("Competitor prices could not be fetched or no competitor prices available.");
                }

                // Calculate price using fetched competitor prices
                var result = _roomService.CalculatePrice(roomType, season, occupancyRate, competitorPrices); // Ensure CalculatePrice method is updated
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        

    }

}
}
