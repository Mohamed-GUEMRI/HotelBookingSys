using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class CompetitorPrice
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string RoomType { get; set; }
        public string Season { get; set; }
        public int OccupancyRate { get; set; }
        public decimal BasePrice { get; set; }
        public string Competitor { get; set; }
        public int ID { get; set; }
        public bool HaveConnectingRooms { get; set; }
        public string AvailableViews { get; set; }
    }

}
