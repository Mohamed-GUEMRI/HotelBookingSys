using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomType { get; set; }  // Standard, Deluxe, Suite
        public decimal BasePrice { get; set; }

        public Room(int id, string roomType, decimal basePrice)
        {
            Id = id;
            RoomType = roomType;
            BasePrice = basePrice;
        }
    }
}
