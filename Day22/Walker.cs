using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    // simple class to bundle row, column, direction
    public class Walker
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Dir { get; set; }    // 0=E, 1=S, 2=W, 3=N

        public Walker(int r, int c, int d)
        {
            Row = r; Col = c; Dir = d;
        }

        public string OrdDir()
        {
            if (Dir == 0) return "E";
            if (Dir == 1) return "S";
            if (Dir == 2) return "W";
            if (Dir == 3) return "N";
            return $"invalid direction: {Dir}";
        }
    }
}
