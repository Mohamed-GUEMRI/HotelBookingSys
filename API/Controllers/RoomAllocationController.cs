using Microsoft.AspNetCore.Mvc;
using RoomAllocationLogic.DTOs;
using RoomAllocationLogic.Services;
using System.Threading.Tasks;

namespace HotelBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomAllocationController : ControllerBase
    {
        private readonly RoomAllocationService _roomAllocationService;

        public RoomAllocationController(RoomAllocationService roomAllocationService)
        {
            _roomAllocationService = roomAllocationService;
        }

        [HttpPost("allocate")]
        public async Task<IActionResult> AllocateRoom([FromBody] BookingRequest bookingRequest)
        {
            if (bookingRequest == null)
            {
                return BadRequest("Booking request is null.");
            }

            try
            {
                var bookingResponse = await _roomAllocationService.AllocateRoom(bookingRequest);
                return Ok(bookingResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Handles invalid room type, for example.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
