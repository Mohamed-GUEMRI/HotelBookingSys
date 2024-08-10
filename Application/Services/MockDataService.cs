using Application.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MockDataService
    {
        private string filePath = "C:\\Users\\HP\\source\\repos\\HotelBookigSystem\\Application\\mock-data\\mockdata.json";
        public List<CompetitorPrice> LoadMockData(string[] competitors, string season, string roomType)
        {
            var json = File.ReadAllText(filePath);
            var allData = JsonConvert.DeserializeObject<List<CompetitorPrice>>(json);

            var filteredData = new List<CompetitorPrice>();

            foreach (var data in allData)
            {
                bool matches = true;

                // Check competitors
                if (competitors != null && competitors.Length > 0)
                {
                    if (!competitors.Contains(data.Competitor, StringComparer.OrdinalIgnoreCase))
                    {
                        matches = false;
                    }
                }

                // Check season
                if (!string.IsNullOrEmpty(season))
                {
                    if (!string.Equals(data.Season, season, StringComparison.OrdinalIgnoreCase))
                    {
                        matches = false;
                    }
                }

                // Check room type
                if (!string.IsNullOrEmpty(roomType))
                {
                    if (!string.Equals(data.RoomType, roomType, StringComparison.OrdinalIgnoreCase))
                    {
                        matches = false;
                    }
                }

                if (matches)
                {
                    filteredData.Add(data);
                }
            }

            return filteredData;
        }

    }

}
