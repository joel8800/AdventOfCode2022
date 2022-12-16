using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    public class Sensor
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int bX { get; set; }
        public int bY { get; set; }
        public int BeaconDist { get; set; }
        public Sensor(int x, int y, int bx, int by)
        {
            X = x;
            Y = y;
            bX = bx;
            bY = by;

            BeaconDist = Math.Abs(X - bx) + Math.Abs(Y - by);
        }
    }
}
