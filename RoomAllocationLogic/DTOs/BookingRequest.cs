using RoomAllocationLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAllocationLogic.DTOs
{
    public class BookingRequest
    {
        public string RoomType { get; set; }
        public int Nights { get; set; }
        public string Season { get; set; }
        public SpecialRequests SpecialRequests { get; set; }
    }
}
