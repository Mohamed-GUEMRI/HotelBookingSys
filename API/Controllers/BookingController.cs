using BookingProcess.Services;
using Microsoft.AspNetCore.Mvc;
using RoomAllocationLogic.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("bookings")]
        public async Task<ActionResult<List<BookingResponse>>> BookRooms([FromBody] List<BookingRequest> bookingRequests)
        {
            if (bookingRequests == null || !bookingRequests.Any())
            {
                return BadRequest("No booking requests provided.");
            }

            var results = _bookingService.ProcessBookings(bookingRequests);

            return Ok(results);
        }
    }
}
