using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    public class Beacon
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Beacon(int x, int y)
        {
            X = x;
            Y = y;
        }

        // override Equals and GetHashCode so we can use a HashSet
        public override bool Equals(Object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Beacon other = (Beacon)obj;
                return (X == other.X && Y == other.Y);
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(X, Y).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format($"[{X},{Y}]");
        }
    }
}
