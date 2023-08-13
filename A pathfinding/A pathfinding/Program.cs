using System.Security.Claims;

namespace A_pathfinding
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = ("A* Pathfinding");
            Map map  = new Map();
            Location location = new Location();
            map.DrawMap();
            location.SetStartingLocation();
            location.GetRandomEndLocation();
            int SLEEP_TIME = 100; 
            //===================================================================
            //      Algoritmen
            //===================================================================
            Location current = null;
            var openList = new List<Location>();
            var closedList = new List<Location>();
            int g = 0;

            // Start med at tilføje den Udgangspunktet (start) til 'openList'
            openList.Add(location.SetStartingLocation());

            while (openList.Count > 0)
            {
                //Få værdien med den laveste F-score
                var lowest = openList.Min(l => l.F);
                current = openList.First(l => l.F == lowest);

                // Tilføj den nuværende 'tile' til 'closedList'
                closedList.Add(current);

                // Viser den nuværende 'tile' på kortet
                Console.SetCursorPosition(current.X, current.Y);
                Console.Write('.');
                Console.SetCursorPosition(current.X, current.Y);
                System.Threading.Thread.Sleep(SLEEP_TIME);

                // fjerner den fra 'openList'  
                openList.Remove(current);

                // hvis vi tilføjer destinationen til 'closedList, har vi fundet ruten;
                if (closedList.FirstOrDefault(l => l.X == location.GetRandomEndLocation().X && l.Y == location.GetRandomEndLocation().Y) != null)
                    break;

                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, map.map, openList);
                g = current.G + 1;

                foreach (var adjacentSquare in adjacentSquares)
                {
                    //hvis den tilstødende 'tile' allerede er i den lukkede liste - ignorer denne
                    if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
                        && l.Y == adjacentSquare.Y) != null)
                        continue;

                    //hvis den ikke er i den åbne liste 
                    if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
                        && l.Y == adjacentSquare.Y) == null)
                    {
                        //Udregn score og set 'parent'  
                        adjacentSquare.G = g;
                        adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, location.GetRandomEndLocation().X, location.GetRandomEndLocation().Y);
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;

                        // Denne tilføjes til den åbne liste
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        //test at hvis der gøres brug af den nuværende tiles G-score gør den
                        //    tilstødende F-score lavere, hvis ja, opdater 'Patrent' da dette betyder
                        //    at det er en bedre rute 
                        if (g + adjacentSquare.H < adjacentSquare.F)
                        {
                            adjacentSquare.G = g;
                            adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                            adjacentSquare.Parent = current;
                        }
                    }
                }
            }

            Location end = current;

            // Denne kode vil erstatte ' . ' med ' _ ' for at illustrere den endelige rute. 
            while (current != null)
            {
                Console.SetCursorPosition(current.X, current.Y);
                Console.Write('_');
                Console.SetCursorPosition(current.X, current.Y);
                current = current.Parent;
                System.Threading.Thread.Sleep(SLEEP_TIME);
            }

            if (end != null)
            {
                Console.SetCursorPosition(0, 20);
                Console.WriteLine("Path : {0}", end.G);
            }

            // slutteligt udskrives rutens længde  
            Console.ReadLine();
        }

        static List<Location> GetWalkableAdjacentSquares(int x, int y, string[] map, List<Location> openList)
        {
            List<Location> list = new List<Location>();

            if (map[y - 1][x] == ' ' || map[y - 1][x] == 'B')
            {
                Location node = openList.Find(l => l.X == x && l.Y == y - 1);
                if (node == null) list.Add(new Location() { X = x, Y = y - 1 });
                else list.Add(node);
            }

            if (map[y + 1][x] == ' ' || map[y + 1][x] == 'B')
            {
                Location node = openList.Find(l => l.X == x && l.Y == y + 1);
                if (node == null) list.Add(new Location() { X = x, Y = y + 1 });
                else list.Add(node);
            }

            if (map[y][x - 1] == ' ' || map[y][x - 1] == 'B')
            {
                Location node = openList.Find(l => l.X == x - 1 && l.Y == y);
                if (node == null) list.Add(new Location() { X = x - 1, Y = y });
                else list.Add(node);
            }

            if (map[y][x + 1] == ' ' || map[y][x + 1] == 'B')
            {
                Location node = openList.Find(l => l.X == x + 1 && l.Y == y);
                if (node == null) list.Add(new Location() { X = x + 1, Y = y });
                else list.Add(node);
            }

            return list;
        }

        static int ComputeHScore(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }
    }


}

