using RoomAllocationLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAllocationLogic.DTOs
{
    public class BookingResponse
    {
        public string RoomType { get; set; }
        public int AllocatedRoomId { get; set; }
        public decimal TotalPrice { get; set; }
        public SpecialRequests SpecialRequests { get; set; }
    }
}
