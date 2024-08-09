using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string RoomType { get; set; }
        public decimal BasePrice { get; set; }
        public decimal adjustedPrice { get; set; }
    }
}
