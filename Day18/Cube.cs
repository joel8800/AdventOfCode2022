using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    public class Cube : IComparable<Cube>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int Exposed { get; set; }

        public Cube(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            Exposed = 6;
        }

        public override string ToString()
        {
            return $"[{X},{Y},{Z}]";
        }

        //override Equals and GetHashCode so we can use a HashSet
        public override bool Equals(Object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Cube other = (Cube)obj;
                return (X == other.X && Y == other.Y && Z == other.Z);
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(X, Y, Z).GetHashCode();
        }

        // public int CompareTo(Cube other) => (X, Y, Z).CompareTo((other.X, other.Y, other.Z));

        public int CompareTo(Cube? other)
        {
            if (other == null) return 1;

            if (X < other.X)
                return -1;
            if (X > other.X)
                return 1;
            if (X == other.X)
            {
                if (Y < other.Y)
                    return -1;
                if (Y > other.Y)
                    return 1;
                if (Y == other.Y)
                {
                    if (Z < other.Z)
                        return -1;
                    if (Z > other.Z)
                        return 1;
                    return 0;
                }
            }
            return 0;
        }
    }
}
