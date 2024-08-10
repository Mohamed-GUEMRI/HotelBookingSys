using Application.DTOs;
using Application.Entities;
using Application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Application.Services
{
    public class RoomService : IRoomService
    {
        private List<CompetitorPrice> competitorPrices;
        private readonly MockDataService mockDataService;
        private readonly HttpClient _httpClient;

        public RoomService(IHttpClientFactory httpClientFactory)
        {
            mockDataService = new MockDataService(); // Initialize MockDataService
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<CompetitorPrice>> GetCompetitorPrices(string season, string roomType, string[] competitors)
        {
            var requestUri = $"https://localhost:7277/api/Competitors/prices?season={season}&roomType={roomType}";
            var requestBody = JsonConvert.SerializeObject(competitors);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            var response = await _httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode(); // Throws if not a success code.

            var responseContent = await response.Content.ReadAsStringAsync();
            var competitorPrices = JsonConvert.DeserializeObject<List<CompetitorPrice>>(responseContent);
            return competitorPrices;
        }

        public RoomDTO CalculatePrice(string roomType, string season, int occupancyRate, List<CompetitorPrice> competitorPrices)
        {
            // Load mock data filtered by competitors, season, and room type
            this.competitorPrices = competitorPrices;

            RoomDTO roomDTO = new RoomDTO
            {
                RoomType = roomType,
                BasePrice = roomType.ToLower() switch
                {
                    "standard" => 100m,
                    "deluxe" => 150m,
                    "suite" => 250m,
                    _ => throw new ArgumentException("Invalid room type")
                }
            };

            // Apply seasonality adjustment
            decimal seasonalityFactor = season.ToLower() switch
            {
                "offseason" => -0.20m,
                "peakseason" => 0.30m,
                _ => 0m
            };
            decimal priceAfterSeasonality = roomDTO.BasePrice * (1 + seasonalityFactor);

            // Apply occupancy rate adjustment
            decimal occupancyFactor = occupancyRate switch
            {
                int rate when rate >= 0 && rate <= 30 => -0.10m,
                int rate when rate > 30 && rate <= 70 => 0m,
                int rate when rate > 70 && rate <= 100 => 0.20m,
                _ => throw new ArgumentException("Invalid occupancy rate")
            };
            decimal priceAfterOccupancy = priceAfterSeasonality * (1 + occupancyFactor);

            // Apply competitor pricing adjustment
            if (competitorPrices != null && competitorPrices.Count > 0)
            {
                // Calculate the average competitor price
                decimal averageCompetitorPrice = competitorPrices
                    .Select(price => price.BasePrice) // Use BasePrice from CompetitorPrice
                    .Average();

                decimal competitorAdjustment = 0m;
                if (averageCompetitorPrice < priceAfterOccupancy)
                {
                    competitorAdjustment = -0.10m;
                }
                else if (averageCompetitorPrice >= priceAfterOccupancy)
                {
                    competitorAdjustment = 0.10m;
                }
                priceAfterOccupancy *= (1 + competitorAdjustment);
            }

            // Return the final price, rounded to two decimal places
            roomDTO.adjustedPrice = Math.Round(priceAfterOccupancy, 2);
            return roomDTO;
        }




    }
}
