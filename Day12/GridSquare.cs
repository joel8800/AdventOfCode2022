using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    internal class GridSquare
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Distance { get; set; }

        public GridSquare(int x, int y, int d, int h) 
        {
            X = x;
            Y = y;
            Height = h;
            Distance = d;
        }

        public override bool Equals(Object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                GridSquare other = (GridSquare)obj;
                return (X == other.X && Y == other.Y);
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(X, Y).GetHashCode();
        }
    }

    
}
