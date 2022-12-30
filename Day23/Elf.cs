using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public class Elf
    {
        // current location
        public int X { get; set; }
        public int Y { get; set; }

        // proposed location
        public int Xp { get; set; }
        public int Yp { get; set; }
        public bool WantsToMove { get; set; }
        public bool Proposed { get; set; }

        public Elf(int x, int y) 
        {
            X = x;
            Y = y;
            WantsToMove = false;
            Proposed = false;
        }

        public void ProposeMove(int direction)
        {
            switch (direction)
            {
                case 0: // N
                    Xp = X; Yp = Y + 1; break;
                case 1: // S
                    Xp = X; Yp = Y - 1; break;
                case 2: // W
                    Yp = Y; Xp = X - 1; break;
                case 3: // E
                    Yp = Y; Xp = X + 1; break;
            }
            Proposed = true;
        }

        public void Move()
        {
            X = Xp;
            Y = Yp;
            WantsToMove = false;
        }

    }
}
