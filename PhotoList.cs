using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Static class that stores the pictures of a room.

namespace HotelManagement
{
    public static class PhotoList
    {
        public static List<string> list { get; } = new List<string>();
        public static int index { get; set; }
    }
}
