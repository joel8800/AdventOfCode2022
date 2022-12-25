using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    public class Rock
    {
        static List<HashSet<(int x, long y)>> rockShapes = new()
        {
            new() { (0, 0), (1, 0), (2, 0), (3, 0) },
            new() { (1, 2), (0, 1), (1, 1), (2, 1), (1, 0) },
            new() { (2, 2), (2, 1), (0, 0), (1, 0), (2, 0) },
            new() { (0, 3), (0, 2), (0, 1), (0, 0) },
            new() { (0, 1), (1, 1), (0, 0), (1, 0) }
        };

        public HashSet<(int x, long y)> Parts { get; set; }

        public Rock(long type, long y)
        {
            // create each rock with x = 2, and y = 4 steps above highest point
            Parts = rockShapes[(int)(type % 5)].Select(c => (c.x + 2, c.y + y)).ToHashSet();
        }

        
        // move rock left unless it hits left boundary or another rock
        public void MoveLeft(HashSet<(int x, long y)> column)
        {
            if (Parts.Any(c => c.x == 0))
                return;

            HashSet<(int x, long y)> newPosition = Parts.Select(c => (c.x - 1, c.y)).ToHashSet();

            if (newPosition.Intersect(column).Any() == false)
                Parts = newPosition;    // no collision, commit move right
        }

        // move rock right unless it hits right boundayr or another rock
        public void MoveRight(HashSet<(int x, long y)> column)
        {
            if (Parts.Any(c => c.x == 6))
                return;

            HashSet<(int x, long y)> newPosition = Parts.Select(c => (c.x + 1, c.y)).ToHashSet();

            if (newPosition.Intersect(column).Any() == false)
                Parts = newPosition;    // no collision, commit move right
        }

        // return true if placed
        public bool MoveDown(HashSet<(int x, long y)> column)
        {
            HashSet<(int x, long y)> newPosition = Parts.Select(c => (c.x, c.y - 1)).ToHashSet();
            
            if (newPosition.Intersect(column).Any() == false)
            {
                Parts = newPosition;       // no collision, commit move down
                return false;
            }
            else
            {
                foreach (var p in Parts)  // collision, add original Parts to column
                    column.Add(p);

                return true;
            }
        }

        public void Print()
        {
            foreach (var comp in Parts)
                Console.Write($"{comp}, ");
            Console.WriteLine();
        }
    }
}
