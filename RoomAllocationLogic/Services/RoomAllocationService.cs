using Application.Entities;
using Application.Interfaces;
using Newtonsoft.Json;
using RoomAllocationLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoomAllocationLogic.Services
{
    public class RoomAllocationService
    {
        private readonly IRoomService _roomService;

        // Counters for room allocation
        private int _deluxeRoomCounter = 101;
        private int _standardRoomCounter = 201;
        private int _suiteRoomCounter = 301;

        public RoomAllocationService(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task<BookingResponse> AllocateRoom(BookingRequest request)
        {
            int allocatedRoomId;

            // Allocate room ID based on room type
            switch (request.RoomType.ToLower())
            {
                case "deluxe":
                    allocatedRoomId = _deluxeRoomCounter++;
                    break;
                case "standard":
                    allocatedRoomId = _standardRoomCounter++;
                    break;
                case "suite":
                    allocatedRoomId = _suiteRoomCounter++;
                    break;
                default:
                    throw new ArgumentException("Invalid room type");
            }

            // Get competitor prices 
            var competitorPrices = GetMockCompetitorPrices(request.Season, request.RoomType);

            // Calculate the price using the dynamic pricing engine
            var totalPrice = _roomService.CalculatePrice(
                request.RoomType,
                request.Season,
                GetOccupancyRate(),
                competitorPrices);

            // Return the booking response
            return new BookingResponse
            {
                RoomType = request.RoomType,
                AllocatedRoomId = allocatedRoomId,
                TotalPrice = totalPrice.adjustedPrice * request.Nights, // Ensure this matches your calculation
                SpecialRequests = request.SpecialRequests
            };
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
