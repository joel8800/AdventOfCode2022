using AoCUtils;
using Day17;
using System.Globalization;
using System.Runtime.InteropServices;

Console.WriteLine("Day17: Pyroclastic Flow");

string input = File.ReadAllText("inputSamp.txt");
int inputPtr = 0;

List<int> column = new() { 0, 0, 0 };
int highPoint = 0;

int rockType = 0;
int numRocks = 1;
int rockBottom = 3;

while (numRocks <= 5)
{
    Rock newRock = new(rockType);
    //newRock.Print();

    while (rockBottom > highPoint + 1)
    {
        if (input[inputPtr] == '<')
            newRock.ShiftLeft();
        else
            newRock.ShiftRight();
        //newRock.Print();
        // update inputPtr
        inputPtr = (inputPtr + 1) % input.Length;

        if (ReachedBottom(newRock, highPoint))
        {
            Console.WriteLine("placing rock");
            PlaceRock(newRock, highPoint);
            highPoint += newRock.Height;
        }
        else
        {   // further to drop
            Console.WriteLine("moving rock down 1");
            rockBottom -= 1;
            if (rockBottom == 0)
                PlaceRock(newRock, highPoint);
        }
    }

    Console.WriteLine($"column height: {column.Count}");
    PrintColumn();

    // setup for next rock
    numRocks++;
    rockType = (rockType + 1) % 5;
    rockBottom = highPoint + 3;
}



bool ReachedBottom(Rock rock, int height)
{
    //if (height == 0)
    //    return true;
    
    if ((rock.bits[0] ^ column[height]) == (rock.bits[0] | column[height]))
    {
        if (rock.Type == 1) // check second row of + rock
        {
            if ((rock.bits[1] ^ column[height]) == (rock.bits[1] | column[height]))
                return false;
        }
        return false;
    }
    else
        return true;
}


void PlaceRock(Rock rock, int height)
{
    for (int i = 0; i < rock.Height; i++)
    {
        column.Add(rock.bits[i]);
    }
}

void PrintColumn()
{
    for (int i = column.Count - 1; i >= 0; i--)
    {
        Console.Write('|');
        for (int j = 6; j >= 0; j--)
        {
            int b = (column[i] >> j) & 0x01;
            char bit = (b == 1) ? '#' : '.';
            Console.Write(bit);
        }
        Console.WriteLine('|');
    }
    Console.WriteLine("+-------+");
}
