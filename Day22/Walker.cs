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
        private int _direction;

        public int Row { get; set; }
        public int Col { get; set; }
        public int RowDir { get; set; }
        public int ColDir { get; set; }
        //public int Dir { get; set; }    // 0=E, 1=S, 2=W, 3=N
        public int Dir
        {
            get => _direction;
            set
            {
                _direction = value;
                SetDirection();
            }
        }

        public Walker(int r, int c, int d)
        {
            Row = r; Col = c; Dir = d;
            SetDirection();
        }

        public string OrdDir()
        {
            if (Dir == 0) return "E";
            if (Dir == 1) return "S";
            if (Dir == 2) return "W";
            if (Dir == 3) return "N";
            return $"invalid direction: {Dir}";
        }

        public void SetDirection()
        {
            //Console.WriteLine($"Dir set to {Dir}");
            switch (Dir)
            {
                case 0:   // E
                {
                    RowDir = 0; ColDir = 1; break;
                }
                case 1:   // S
                {
                    RowDir = 1; ColDir = 0; break;
                }
                case 2:   // W
                {
                    RowDir = 0; ColDir = -1; break;
                }
                case 3:   // N
                {
                    RowDir = -1; ColDir = 0; break;
                }
                default:
                    throw new InvalidDataException();
            }   
        }
    }
}
