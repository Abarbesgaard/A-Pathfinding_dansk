using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace A_pathfinding
{
    class Location
    {
        
        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
       
        public Location Parent { get; set; }

        public Location SetStartingLocation()
        {
            var start = new Location { X = 1, Y = 1 };
            return start;
        }

        public Location SetEndLocation()
        {
            var target = new Location { X = 12, Y = 2 };
            return target;
        } 

        public Location GetRandomEndLocation()
        {
            Random random = new Random();
            Map map = new Map();
            int maxX = map.map[0].Length;
            int maxY = map.map.Length;

            int randomX = random.Next(2, maxX);
            int randomY = random.Next(1, maxY);
            Location target = new Location { X = randomX, Y = randomY };
            return target;
        }

    }
}
