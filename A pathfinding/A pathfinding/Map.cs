using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_pathfinding
{
    public class Map
    {
        public string[] map = new string[]
       {
                "+--------------------------------------------+",
                "|        X                                   |",
                "|  X     X                                   |",
                "|  X     X                                   |",
                "|  XX  X   X           X                     |",
                "|  X     X                                   |",
                "|                                 X          |",
                "|                                 X          |",
                "|                      X      X   X          |",
                "|                             X   X          |",
                "|                X            X   X          |",
                "|                              X  X          |",
                "|                                            |",
                "|                                            |",
                "|                                            |",
                "|                                            |",
                "|                                            |",
                "|                                            |",
                "|                                            |",
                "+--------------------------------------------+",
       };

        public void DrawMap()
        {
            foreach (var line in map)
                Console.WriteLine(line);
        }
    }
}
