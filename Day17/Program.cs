using AoCUtils;
using Day17;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Day17: Pyroclastic Flow");

string input = File.ReadAllText("input.txt");
int inputPtr = 0;

// make column and init bottom to height = -1
long highPoint = -1;
HashSet<ValueTuple<int, long>> column = new();
for (int i = 0; i < 7; i++)
{
    ValueTuple<int, long> init = new(i, -1);
    column.Add(init);
}

long rockCount = 0;
long L = 2022;    // 1000000000000;

while (rockCount < L)
{
    int rockType = (int) rockCount % 5;
    Rock r = new(rockType, 0, highPoint + 4);   // gap of 3 between rock and highest point

    bool placed = false;
    while (placed == false)
    {
        // pushed left or right
        if (input[inputPtr] == '<')
            r.MoveLeft(column);
        else
            r.MoveRight(column);

        // then move down
        placed = r.MoveDown(column);
        if (placed)
        {
            highPoint = column.Max(y => y.Item2);
            //PrintColumn(column);
        }
 
        inputPtr = (inputPtr + 1) % input.Length;
    }

    rockCount += 1;
}

Console.WriteLine($"Part1: {highPoint + 1}");   // add 1 as column is 0 based






void PrintColumn(HashSet<ValueTuple<long, long>> column)
{
    long maxY = column.Max(y => y.Item2);

    for (long y = maxY; y >= 0; y--)
    {
        string row = string.Empty;

        for (long x = 0; x < 7; x++)
        {
            if (column.Contains(new ValueTuple<long, long>(x, y)))
                row += "#";
            else
                row += ".";
        }
        Console.WriteLine($"|{row}| {y}");
    }
}



//while (numRocks <= 5)
//{
//    Rock newRock = new(rockCount);
//    //newRock.Print();

//    while (rockBottom > highPoint + 1)
//    {
//        // left or right movement first
//        if (input[inputPtr] == '<')
//            newRock.ShiftLeft();
//        else
//            newRock.ShiftRight();

//        //newRock.Print();
//        // update inputPtr
//        inputPtr = (inputPtr + 1) % input.Length;

//        if (ReachedBottom(newRock, highPoint))
//        {
//            Console.WriteLine("placing rock");
//            PlaceRock(newRock, highPoint);
//            highPoint += newRock.Height;
//        }
//        else
//        {   // further to drop
//            Console.WriteLine("moving rock down 1");
//            rockBottom -= 1;
//            if (rockBottom == 0)
//                PlaceRock(newRock, highPoint);
//        }
//    }

//    Console.WriteLine($"column height: {column.Count}");
//    PrintColumn();

//    // setup for next rock
//    numRocks++;
//    rockCount = (rockCount + 1) % 5;
//    rockBottom = highPoint + 3;
//}



//bool ReachedBottom(Rock rock, int height)
//{
//    //if (height == 0)
//    //    return true;
    
//    if ((rock.bits[0] ^ column[height]) == (rock.bits[0] | column[height]))
//    {
//        if (rock.Type == 1) // check second row of + rock
//        {
//            if ((rock.bits[1] ^ column[height]) == (rock.bits[1] | column[height]))
//                return false;
//        }
//        return false;
//    }
//    else
//        return true;
//}


//void PlaceRock(Rock rock, int height)
//{
//    for (int i = 0; i < rock.Height; i++)
//    {
//        column.Add(rock.bits[i]);
//    }
//}

//void PrintColumn()
//{
//    for (int i = column.Count - 1; i >= 0; i--)
//    {
//        Console.Write('|');
//        for (int j = 6; j >= 0; j--)
//        {
//            int b = (column[i] >> j) & 0x01;
//            char bit = (b == 1) ? '#' : '.';
//            Console.Write(bit);
//        }
//        Console.WriteLine('|');
//    }
//    Console.WriteLine("+-------+");
//}




