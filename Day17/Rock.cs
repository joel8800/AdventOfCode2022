using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    public class Rock
    {
        public int X { get; set; }
        public long Y { get; set; }
        public HashSet<ValueTuple<int, long>> set { get; set; }

        public Rock(int type, int x, long y)
        {
            X = x;
            Y = y;
            set = new();
            GetPieces(type, y);
        }

        // create each rock with x = 2, and y = 4 steps above highest point
        public void GetPieces(int type, long y)
        {
            switch (type)
            {
                case 0:     // -
                    set = new(){ new(2, y), new(3, y), new(4, y), new(5, y) };
                    break;
                case 1:     // +
                    set = new() { new(3, y + 2), new(2, y + 1), new(3, y + 1), new(4, y + 1), new(3, y) };
                    break;
                case 2:     // backward L
                    set = new() { new(2, y), new(3, y), new(4, y), new(4, y + 1), new(4, y + 2) };
                    break;
                case 3:     // I
                    set = new() { new(2, y), new(2, y + 1), new(2, y + 2), new(2, y + 3) };
                    break;
                case 4:     // o
                    set = new() { new(2, y), new(2, y + 1), new(3, y), new(3, y + 1) };
                    break;
            }
        }

        // move rock left unless it hits left boundary or another rock
        public void MoveLeft(HashSet<ValueTuple<int, long>> column)
        {
            HashSet<ValueTuple<int, long>> newPosition = new();

            foreach (var p in set)
            {
                ValueTuple<int, long> np = new(p.Item1 - 1, p.Item2);
                newPosition.Add(np);
            }

            if (newPosition.All(x => x.Item1 >= 0))
                if (newPosition.Intersect(column).Any() == false)
                    set = newPosition;   // no collision, commit move left
        }

        // move rock right unless it hits right boundayr or another rock
        public void MoveRight(HashSet<ValueTuple<int, long>> column)
        {
            HashSet<ValueTuple<int, long>> newPosition = new();

            foreach (var p in set)
            {
                ValueTuple<int, long> np = new(p.Item1 + 1, p.Item2);
                newPosition.Add(np);
            }

            if (newPosition.All(x => x.Item1 < 7))
                if (newPosition.Intersect(column).Any() == false)
                    set = newPosition;   // no collision, commit move right
        }

        // return true if placed
        public bool MoveDown(HashSet<ValueTuple<int, long>> column)
        {
            HashSet<ValueTuple<int, long>> newPosition = new();

            foreach (var p in set)
            {
                ValueTuple<int, long> np = new(p.Item1, p.Item2 - 1);
                newPosition.Add(np);
            }
            
            if (newPosition.Intersect(column).Any() == false)
            {
                set = newPosition;       // no collision, commit move down
                return false;
            }
            else
            {
                foreach (var p in set)  // collision, add original set to column
                    column.Add(p);

                return true;
            }
        }

        // not used, but written just in case
        public void MoveUp(HashSet<ValueTuple<int, long>> column)
        {
            HashSet<ValueTuple<int, long>> ns = new();

            foreach (var p in set)
            {
                ValueTuple<int, long> np = new(p.Item1, p.Item2 + 1);
                ns.Add(np);
            }
            set = ns;
        }

        public void Print()
        {
            foreach (var comp in set)
                Console.Write($"{comp}, ");
            Console.WriteLine();
        }


        //public int Type { get; set; }
        //public int Height { get; set; } 
        //public int[] ?bits { get; set; }
        

        //// 0 = -    1x4
        //// 1 = +    3x3
        //// 2 = L    3x3     <- backwards L
        //// 3 = I    4x1
        //// 4 = o    2x2

        //public Rock(int type)
        //{
        //    bits = new int[3];
        //    Type = type;
        //    switch (type)
        //    {
        //        case 0:
        //            bits = new int[1];   // -
        //            bits[0] = 0x1e;       // 001 1110
        //            Height = 1;
        //            break;
        //        case 1:
        //            bits = new int[3];   // +
        //            bits[0] =0x08;        // 000 1000
        //            bits[1] =0x1c;        // 001 1100
        //            bits[2] =0x08;        // 000 1000
        //            Height = 3;
        //            break;
        //        case 2:
        //            bits = new int[3];   // bkwd L
        //            bits[0] = 0x1c;       // 001 1100
        //            bits[1] = 0x04;       // 000 0100
        //            bits[2] = 0x04;       // 000 0100
        //            Height = 3;
        //            break;
        //        case 3:
        //            bits = new int[4];   // I
        //            bits[0] = 0x10;       // 001 0000          
        //            bits[1] = 0x10;       // 001 0000
        //            bits[2] = 0x10;       // 001 0000
        //            bits[3] = 0x10;       // 001 0000
        //            Height = 4;
        //            break;
        //        case 4:
        //            bits = new int[2];   // o
        //            bits[0] = 0x18;       // 001 1000
        //            bits[1] = 0x18;       // 001 1000
        //            Height = 2;
        //            break;
        //    }
        //}
        
        //public bool ShiftLeft()
        //{
        //    Console.WriteLine($"Rock type {Type} - shift left");
        //    //if (bits?.Any(x => (x & 0x40) == 0x40))
        //    //    return false;

        //    for (int i = 0; i < Height; i++)
        //    {
        //        bits[i] <<= 1;
        //    }
        //    return true;
        //}

        //public bool ShiftRight()
        //{
        //    Console.WriteLine($"Rock type {Type} - shift right");
        //    //if (bits?.Any(x => (x & 0x01) == 0x01))
        //    //    return false;

        //    for (int i = 0; i < Height; i++)
        //    {
        //        bits[i] >>= 1;
        //    }
        //    return true;
        //}

        //public void Print()
        //{
        //    char bit;

        //    for (int i = 0; i < Height; i++)
        //    {
        //        for (int j = 6; j >= 0; j--)
        //        {
        //            bit = (bits?[i] & (1 << j)) > 0 ? '1' : '0';
        //            Console.Write(bit);
        //        }
        //        Console.WriteLine();
        //    }
        //}
    }
}
