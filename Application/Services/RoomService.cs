using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RoomService : IRoomService
    {
        

        public RoomService()
        {
        }
        public RoomDTO CalculatePrice(string roomType, string season, int occupancyRate, string[] competitorPrices)
        {
            RoomDTO RoomDTO = new RoomDTO();
            RoomDTO.RoomType = roomType;
            RoomDTO.BasePrice =  roomType.ToLower() switch
            {
                "standard" => 100m,
                "deluxe" => 150m,
                "suite" => 250m,
                _ => throw new ArgumentException("Invalid room type")
            };

            // 2. Apply seasonality adjustment
            decimal seasonalityFactor = season.ToLower() switch
            {
                "offseason" => -0.20m,
                "peakseason" => 0.30m,
                _ => 0m
            };
            decimal priceAfterSeasonality = RoomDTO.BasePrice * (1 + seasonalityFactor);

            // 3. Apply occupancy rate adjustment
            decimal occupancyFactor = occupancyRate switch
            {
                int rate when rate >= 0 && rate <= 30 => -0.10m,
                int rate when rate > 30 && rate <= 70 => 0m,
                int rate when rate > 70 && rate <= 100 => 0.20m,
                _ => throw new ArgumentException("Invalid occupancy rate")
            };
            decimal priceAfterOccupancy = priceAfterSeasonality * (1 + occupancyFactor);

            // 4. Apply competitor pricing adjustment
            if (competitorPrices != null && competitorPrices.Length > 0)
            {
                // Calculate the average competitor price
                decimal averageCompetitorPrice = competitorPrices
                    .Select(price => decimal.TryParse(price, out var parsedPrice) ? parsedPrice : 0m)
                    .Average();

                decimal competitorAdjustment = 0m;
                if (averageCompetitorPrice < priceAfterOccupancy)
                {
                    competitorAdjustment = -0.10m;
                }
                else if (averageCompetitorPrice > priceAfterOccupancy)
                {
                    competitorAdjustment = 0.10m;
                }
                priceAfterOccupancy *= (1 + competitorAdjustment);
            }

            // 5. Return the final price, rounded to two decimal places
            RoomDTO.adjustedPrice = Math.Round(priceAfterOccupancy, 2);
            return RoomDTO;
        }


    }
}
