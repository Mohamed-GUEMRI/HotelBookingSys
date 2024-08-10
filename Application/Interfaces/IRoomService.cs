using Application.DTOs;
using Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRoomService
    {
        RoomDTO CalculatePrice(string roomType, string season, int occupancyRate, List<CompetitorPrice> competitorPrices);
        Task<List<CompetitorPrice>> GetCompetitorPrices(string season, string roomType, string[] competitors);
    }
}
