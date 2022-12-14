using AoCUtils;
using System.Drawing;
using System.Runtime.CompilerServices;

Console.WriteLine("Day14: Regolith Reservoir");

string[] input = FileUtil.ReadFileByLine("input.txt");

char[,] slice = new char[1000, 1000];
int floor = 1000;

for (int i = 0; i < 1000; i++)
    for (int j = 0; j < 1000; j++)
        slice[i, j] = '.';

int maxY = 0;
foreach (string line in input)
{
    string[] points = line.Split("->", StringSplitOptions.TrimEntries);

    string[] coords = points[0].Split(',');
    int x_st = int.Parse(coords[0]);
    int y_st = int.Parse(coords[1]);

    
    for (int i = 1; i < points.Length; i++)
    {
        coords = points[i].Split(',');
        int x_sp = int.Parse(coords[0]);
        int y_sp = int.Parse(coords[1]);

        if (x_st == x_sp)
        {
            int dir = y_st < y_sp ? 1 : -1;
            int y = y_st;

            while (y != y_sp)
            {
                slice[x_st, y] = '#';
                y += dir;
            }
            slice[x_sp, y_sp] = '#';
        }

        if (y_st == y_sp)
        {
            int dir = x_st < x_sp ? 1 : -1;
            int x = x_st;

            while (x != x_sp)
            {
                slice[x, y_st] = '#';
                x += dir;
            }
            slice[x_sp, y_sp] = '#';
        }

        x_st = x_sp;
        y_st = y_sp;

        maxY = y_st > maxY ? y_st : maxY;
        maxY = y_sp > maxY ? y_sp : maxY;
    }

}

floor = maxY + 2;
Console.WriteLine($"floor = {floor}");

for (int i = 0; i < 1000; i++)
    slice[i, floor] = '#';


List<int> markers = new() { 1, 2, 5, 22, 23, 24 };


int count = 0;
while (true)
{
    int x, y;

    count += 1;
    //Console.WriteLine($"count:{count}");

    (x, y) = DropSand(500, 0);

    if (y >= floor - 1)
        break;
    else
        slice[x, y] = 'o';

    //if (markers.Exists(x => x == count))
    //    Print();
}

//Print(490, 510, 0, 15);
//Print(450, 550, 0, 171);
Console.WriteLine($"Part1: {count - 1}");

while (true)
{
    int x, y;

    count += 1;
    //Console.WriteLine($"count:{count}");

    (x, y) = DropSand2(500, 0);

    if (x == 500 && y == 0)
    {
        slice[x, y] = 'o'; 
        break;
    }
    else
        slice[x, y] = 'o';
}

//Print(485, 515, 0, 15);
//Print(320, 680, 0, 171);
Console.WriteLine($"Part2: {count + 1}");  // +1 for blocked on coming to rest?




(int x, int y) DropSand (int x_st, int y_st)
{
    int x = x_st;
    int y = y_st;

    bool roomToFall = true;

    while (roomToFall)
    {
        if (slice[x, y + 1] != '.' && slice[x - 1, y + 1] != '.' && slice[x+1, y+1] != '.')
            break;

        // check directly below
        while (slice[x, y + 1] == '.')
        {
            if (++y >= floor - 2)
            {
                slice[x, y] = 'o';
                roomToFall = false;
                break;
            }
        }

        // look left
        if (slice[x - 1, y + 1] == '.')
        {
            x--; y++;
            continue;
        }
        else if (slice[x + 1, y + 1] == '.')
        { 
            x++; y++;
            continue;
        }
    }

    return (x, y);
}

(int x, int y) DropSand2(int x_st, int y_st)
{
    int x = x_st;
    int y = y_st;

    bool roomToFall = true;

    while (roomToFall)
    {
        if (slice[x, y + 1] != '.' && slice[x - 1, y + 1] != '.' && slice[x + 1, y + 1] != '.')
            break;

        // check directly below
        //while (slice[x, y + 1] == '.')
        //{
        //    if (++y >= floor - 2)
        //    {
        //        slice[x, y] = 'o';
        //        roomToFall = false;
        //        break;
        //    }
        //}
        if (slice[x, y + 1] == '.')
        {
            y++;
            continue;
        }

        // look left
        if (slice[x - 1, y + 1] == '.')
        {
            x--; y++;
            continue;
        }
        else if (slice[x + 1, y + 1] == '.')
        {
            x++; y++;
            continue;
        }
    }

    return (x, y);
}

void Print(int x1, int x2, int y1, int y2)
{
    for (int y = y1; y < y2; y++)
    {
        for (int x = x1; x < x2; x++)
        {
            Console.Write(slice[x, y]);
        }
        Console.WriteLine();
    }
}