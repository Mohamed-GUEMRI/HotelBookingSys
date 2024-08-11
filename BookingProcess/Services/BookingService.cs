using Application.Entities;
using Application.Interfaces;
using Application.Services;
using RoomAllocationLogic.DTOs;
using RoomAllocationLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingProcess.Services
{
    public class BookingService
    {
        private readonly RoomAllocationService _roomAllocationService;
        private readonly IRoomService _pricingService;

        public BookingService(RoomAllocationService roomAllocationService, IRoomService pricingService)
        {
            _roomAllocationService = roomAllocationService;
            _pricingService = pricingService;
        }

        public List<BookingResponse> ProcessBookings(List<BookingRequest> bookingRequests)
        {
            var responses = new List<BookingResponse>();

            foreach (var request in bookingRequests)
            {
                            var competitorPrices = GetMockCompetitorPrices(request.Season, request.RoomType);

                var room = _roomAllocationService.AllocateRoom(request);
                var totalPrice = _pricingService.CalculatePrice(
                request.RoomType,
                request.Season,
                GetOccupancyRate(),
                competitorPrices);

                var response = new BookingResponse
                {
                    RoomType = request.RoomType,
                    AllocatedRoomId = room.Id,
                    TotalPrice = totalPrice.adjustedPrice * request.Nights,
                    SpecialRequests = request.SpecialRequests
                };

                responses.Add(response);
            }

            return responses;
        }
        private int GetOccupancyRate()
        {
            return 50; // Example occupancy rate; adjust as needed
        }

        private List<CompetitorPrice> GetMockCompetitorPrices(string season, string roomType)
        {
            // Mock data
            var mockCompetitorPrices = new List<CompetitorPrice>
            {
                new CompetitorPrice
                {
                    Title = "Deluxe Room Special",
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    RoomType = "Deluxe",
                    Season = season,
                    OccupancyRate = 80,
                    BasePrice = 120.00m,
                    Competitor = "Competitor A",
                    ID = 1,
                    HaveConnectingRooms = true,
                    AvailableViews = "Ocean, Garden"
                },
                new CompetitorPrice
                {
                    Title = "Standard Room Deal",
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    RoomType = "Standard",
                    Season = season,
                    OccupancyRate = 70,
                    BasePrice = 100.00m,
                    Competitor = "Competitor B",
                    ID = 2,
                    HaveConnectingRooms = false,
                    AvailableViews = "City, Garden"
                },
                new CompetitorPrice
                {
                    Title = "Suite Room Premium",
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    RoomType = "Suite",
                    Season = season,
                    OccupancyRate = 90,
                    BasePrice = 150.00m,
                    Competitor = "Competitor C",
                    ID = 3,
                    HaveConnectingRooms = true,
                    AvailableViews = "Ocean, City"
                }
            };

            // Optionally, filter or modify mock data based on the `season` and `roomType` parameters
            // This is a static example; you can adjust it to better match your needs

            return mockCompetitorPrices;
        }
    }

}
