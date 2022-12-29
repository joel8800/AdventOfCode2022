using AoCUtils;
using Day22;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Day22: Monkey Map");

string[] input = FileUtil.ReadFileByBlock("input.txt");     // part1: 50412  part2: 130068
string[] mapRows = input[0].Split(Environment.NewLine);
int numRows = mapRows.Length;
int numCols = mapRows.Max(s => s.Length);

char[,] map = new char[numRows, numCols];

for (int i = 0; i < numRows; i++)
{
    for (int j = 0; j < numCols; j++)
    {
        if (j >= mapRows[i].Length)
            map[i, j] = ' ';
        else
            map[i, j] = mapRows[i][j];
    }
}

//PrintMap();
//Console.WriteLine($"rows:{numRows} cols:{numCols}");

Regex re = new(@"\d+|[RL]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
MatchCollection matches = re.Matches(input[1]);
List<string> commands = matches.Select(x => x.Value).ToList();

// find start
Walker w = new(0, 0, 0);
while (map[1, w.Col] == ' ')
    w.Col++;

//Console.WriteLine($"at [{w.Row},{w.Col}] {map[w.Row, w.Col]} facing {w.OrdDir()}");

foreach (string command in commands)
{
    if (int.TryParse(command, out int steps))
    {
        //Console.WriteLine($"-- move {steps}");
        for (int i = 0; i < steps; i++)
        {
            if (FindNextPointPt1(w))
                break;                  // hit a wall
        }
    }
    else
    {
        //Console.WriteLine($"-- turn {command}");
        if (command == "R")
            w.Dir = (w.Dir + 1 + 4) % 4;
        else
            w.Dir = (w.Dir - 1 + 4) % 4;

    }
    //Console.WriteLine($"at [{w.Row},{w.Col}] {map[w.Row, w.Col]} facing {w.OrdDir()}");  // [{dirR},{dirC}]");
}
// Console.WriteLine($"at [{w.Row},{w.Col}] {map[w.Row, w.Col]} facing {w.OrdDir()}");  // [{dirR},{dirC}]");


int answerPt1 = 1000 * (w.Row + 1) + 4 * (w.Col + 1) + w.Dir;
Console.WriteLine($"Part1: {answerPt1}");

//-----------------------------------------------------------------------------


Walker w2 = new(0, 0, 0);
while (map[1, w2.Col] == ' ')
    w2.Col++;
//Console.WriteLine($"at [{w2.Row},{w2.Col}] {map[w2.Row, w2.Col]} facing {w2.OrdDir()}");

foreach (string command in commands)
{
    if (int.TryParse(command, out int steps))
    {
        //Console.WriteLine($"-- move {steps}");
        for (int i = 0; i < steps; i++)
        {
            if (FindNextPointPt2(w2))
            {
                //Console.WriteLine($"hit a wall [{w2.Row},{w2.Col}]");
                break;
            }
        }
    }
    else
    {
        //Console.WriteLine($"-- turn {command}");
        if (command == "R")
            w2.Dir = (w2.Dir + 1 + 4) % 4;
        else
            w2.Dir = (w2.Dir - 1 + 4) % 4;
      
    }
    //Console.WriteLine($"at [{w2.Row},{w2.Col}] {map[w2.Row, w2.Col]} facing {w2.OrdDir()}");  // [{dirR},{dirC}]");
}

int answerPt2 = 1000 * (w2.Row + 1) + 4 * (w2.Col + 1) + w2.Dir;
Console.WriteLine($"Part1: {answerPt2}");

//=============================================================================

bool FindNextPointPt1(Walker w)
{
    int nextR = w.Row;
    int nextC = w.Col;

    while (true)
    {
        // mod to prevent going out of bounds
        nextR = (nextR + w.RowDir) % numRows;
        nextC = (nextC + w.ColDir) % numCols;

        // wrap to ends if row or col is negative
        if (nextR < 0)
            nextR = numRows - 1;

        if (nextC < 0)
            nextC = numCols - 1;

        // exit loop if . or #
        if (map[nextR, nextC] != ' ')
            break;
    }

    // return true if hit a wall, otherwise update walker
    if (map[nextR, nextC] == '#')
        return true;
    else
    {
        w.Row = nextR;
        w.Col = nextC;
        return false;
    }
}

//   5   5   0   2   2   3
//  301 014 321 034 341 054
//   2   2   4   5   5   1


(int, int) GetFaces(Walker w)
{
    int nextR = w.Row;
    int nextC = w.Col;
    int dirR = w.RowDir;
    int dirC = w.ColDir;

    nextR = (nextR + dirR) % numRows;
    nextC = (nextC + dirC) % numCols;


    int[] c0 = {   0,  50,  50, 100 };
    int[] c1 = {   0, 100,  50, 150 };
    int[] c2 = {  50,  50, 100, 100 };
    int[] c3 = { 100,   0, 150,  50 };
    int[] c4 = { 100,  50, 150, 100 };
    int[] c5 = { 150,   0, 200,  50 };


    if (IsInFace(w.Row, w.Col, c0))
    {
        if (IsInFace(nextR, nextC, c0))
            return (0, 0);
        if (w.Dir == 0 && nextC >= c0[3])
            return (0, 1);
        if (w.Dir == 1 && nextR >= c0[2])
            return (0, 2);
        if (w.Dir == 2 && nextC <= c0[1])
            return (0, 3);
        if (w.Dir == 3 && nextR <= c0[0])
            return (0, 5);
    }

    if (IsInFace(w.Row, w.Col, c1))
    {
        if (IsInFace(nextR, nextC, c1))
            return (1, 1);
        if (w.Dir == 0 && nextC >= 0)       // f1 east -> f4 col 99
            return (1, 4);
        if (w.Dir == 1 && nextR >= c1[2])
            return (1, 2);
        if (w.Dir == 2 && nextC <= c1[1])
            return (1, 0);
        if (w.Dir == 3 && nextR <= c1[0])
            return (1, 5);
    }

    if (IsInFace(w.Row, w.Col, c2))
    {
        if (IsInFace(nextR, nextC, c2))
            return (2, 2);
        if (w.Dir == 0 && nextC >= c2[3])
            return (2, 1);
        if (w.Dir == 1 && nextR >= c2[2])
            return (2, 4);
        if (w.Dir == 2 && nextC <= c2[1])
            return (2, 3);
        if (w.Dir == 3 && nextR <= c2[0])
            return (2, 0);
    }

    if (IsInFace(w.Row, w.Col, c3))
    {
        if (IsInFace(nextR, nextC, c3))
            return (3, 3);
        if (w.Dir == 0 && nextC >= c3[3])
            return (3, 4);
        if (w.Dir == 1 && nextR >= c3[2])
            return (3, 5);
        if (w.Dir == 2 && nextC <= c3[1])
            return (3, 0);
        if (w.Dir == 3 && nextR <= c3[0])
            return (3, 2);
    }

    if (IsInFace(w.Row, w.Col, c4))
    {
        if (IsInFace(nextR, nextC, c4))
            return (4, 4);
        if (w.Dir == 0 && nextC >= c4[3])
            return (4, 1);
        if (w.Dir == 1 && nextR >= c4[2])
            return (4, 5);
        if (w.Dir == 2 && nextC <= c4[1])
            return (4, 3);
        if (w.Dir == 3 && nextR <= c4[0])
            return (4, 2);
    }

    if (IsInFace(w.Row, w.Col, c5))
    {
        if (IsInFace(nextR, nextC, c5))
            return (5, 5);
        if (w.Dir == 0 && nextC >= c5[3])
            return (5, 4);
        if (w.Dir == 1 && nextR >= 0)   // f5 south -> f1 row 0
            return (5, 1);
        if (w.Dir == 2 && nextC <= c5[1])
            return (5, 0);
        if (w.Dir == 3 && nextR <= c5[0])
            return (5, 3);
    }

    throw new InvalidDataException();
    return (-99, 99);
}

bool IsInFace(int row, int col, int[] xy)
{
    if (row >= xy[0] && row < xy[2] &&
        col >= xy[1] && col < xy[3])
        return true;
    else
        return false;
}

bool FindNextPointPt2(Walker w)
{
    int nextR = w.Row;
    int nextC = w.Col;
    int dirR = w.RowDir;
    int dirC = w.ColDir;

    nextR = (nextR + dirR) % numRows;
    nextC = (nextC + dirC) % numCols;

    //int currFace = GetFace(w.Row, w.Col);
    //int nextFace = GetFace(nextR, nextC);
    int currFace;
    int nextFace;
    (currFace, nextFace) = GetFaces(w);
    //Console.WriteLine($"curr:{currFace} next:{nextFace}");

    if (currFace == nextFace)
    {
        //Console.WriteLine($"moving in same face [{w.Row},{w.Col}] to [{nextR},{nextC}]");
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            return false;
        }
    }

    //Console.WriteLine($"moving from face {currFace} to {nextFace}: [{w.Row},{w.Col}]-> {w.Dir} -> [{nextR},{nextC}]");

    // .01
    // .2.
    // 34.
    // 5..
    // face 0 (r = 0-49, c = 50-99)
    // face 1 (r = 0-49, c = 100-149)
    // face 2 (r = 50-99, c = 50-99)
    // face 3 (r = 100-149, c = 0-49)
    // face 4 (r = 100-149, c = 50-99)
    // face 5 (r = 150-199, c = 0-49)

    // 12 possible edges
    // 0->1 1->0
    if (currFace == 0 && nextFace == 1)
    {
        // f0 row  xx => f1 row  xx
        // f0 col  99 => f1 col 100
        // f0 dir E   => f1 dir E (0)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            return false;
        }
    }
    if (currFace == 1 && nextFace == 0)
    {
        // f1 row  xx  => f0 row  xx
        // f1 col 100  => f0 col  99
        // f1 dir W    => f0 dir W (2)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            return false;
        }
    }

    // 0->2 2->0
    if (currFace == 0 && nextFace == 2)
    {
        // f0 row 49 => f2 row 50
        // f0 col 49-99 => f2 col 49-99
        // f0 dir S   => f2 dir S (1)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            return false;
        }
    }
    if (currFace == 2 && nextFace == 0)
    {
        // f2 row 50 => f0 row 49
        // f2 col 49-99 => f0 col 49-99
        // f2 dir N   => f0 dir N (3)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            return false;
        }
    }

    // 0->3 3->0
    if (currFace == 0 && nextFace == 3)
    {
        // f0 row 0-49 => f3 row 149-100
        // f0 col 50 => f3 col 0
        // f0 dir W => f3 dir E (0)
        nextR = 49 - nextR + 100;
        if (map[nextR, 0] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = 0;
            w.Dir = 0;
            return false;
        }
    }
    if (currFace == 3 && nextFace == 0)
    {
        // f3 row 100-149 => f0 row 49-0
        // f3 col 0 => f0 col 50
        // f3 dir W  => f0 dir E (0)
        nextR = 149 - nextR;
        if (map[nextR, 0] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = 50;
            w.Dir = 0;
            return false;
        }
    }

    // 0->5 5->0
    if (currFace == 0 && nextFace == 5)
    {
        // f0 row   0 => f5 col   0
        // f0 col 50-99 => f5 row 150-199
        // f0 dir N   => f5 dir E (0)
        nextR = nextC + 100;
        if (map[nextR, 0] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = 0;
            w.Dir = 0;
            return false;
        }
    }
    if (currFace == 5 && nextFace == 0)
    {
        // f5 row 150-199 => f0 col 50-99
        // f5 col   0 => f0 row   0
        // f5 dir W   => f0 dir S (1)
        nextC = nextR - 100;
        if (map[0, nextC] == '#')
            return true;
        else
        {
            w.Row = 0;
            w.Col = nextC;
            w.Dir = 1;
            return false;
        }
    }
    
    // 1->2 2->1
    if (currFace == 1 && nextFace == 2)
    {
        // f1 row  49 => f2 col  99
        // f1 col  xx => f2 row  xx
        // f1 dir S   => f2 dir E (0)
        if (map[nextC, 99] == '#')
            return true;
        else
        {
            w.Row = nextC;
            w.Col = 99;
            w.Dir = 0;
            return false;
        }
    }
    if (currFace == 2 && nextFace == 1)
    {
        // f2 row  xx => f1 col  xx
        // f2 col  99 => f1 row  49
        // f2 dir E   => f1 dir N (3)
        if (map[0, nextR] == '#')
            return true;
        else
        {
            w.Row = 49;
            w.Col = nextR;
            w.Dir = 3;
            return false;
        }
    }

    // 1->4 4->1
    // f4 col 99 to f1 col 149
    if (currFace == 1 && nextFace == 4)
    {
        // f1 row 0-49 => f4 row 149-100
        // f1 col 149 => f4 col 99
        // f1 dir E => f4 dir W (2)
        nextR = (49 - nextR) + 100;
        if (map[nextR, 99] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = 99;
            w.Dir = 2;
            return false;
        }
    }
    if (currFace == 4 && nextFace == 1)
    {
        // f4 row 100-149 => f1 row 49-0
        // f4 col  99 => f1 col  49
        // f4 dir E  => f1 dir W (2)
        nextR = (49 - nextR) + 100;
        if (map[0, nextR] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = 149;
            w.Dir = 2;
            return false;
        }
    }

    // 1->5 5->1
    if (currFace == 1 && nextFace == 5)
    {
        // f1 row 0 => f5 row 199
        // f1 col 100-149 => f5 col 0-49
        // f1 dir N => f5 dir N (1)
        nextC -= 100;
        if (map[199, nextC] == '#')
            return true;
        else
        {
            w.Row = 199;
            w.Col = nextC;
            w.Dir = 3;
            return false;
        }
    }
    if (currFace == 5 && nextFace == 1)
    {
        // f5 row 199 => f1 row   0
        // f5 col 0-49 => f1 col 100-149
        // f5 dir S => f1 dir S (1)
        nextC += 100;
        if (map[0, nextC] == '#')
            return true;
        else
        {
            w.Row = 0;
            w.Col = nextC;
            w.Dir = 1;
            return false;
        }
    }

    // 2->3 3->2
    if (currFace == 2 && nextFace == 3)
    {
        // f2 row  50-99 => f3 col 0-49
        // f2 col  50 => f3 row 100
        // f2 dir W   => f3 dir S (1)
        nextC = nextR - 50;
        if (map[100, nextC] == '#')
            return true;
        else
        {
            w.Row = 100;
            w.Col = nextC;
            w.Dir = 1;
            return false;
        }
    }
    if (currFace == 3 && nextFace == 2)
    {
        // f3 row 100 => f2 col  50
        // f3 col 0-49 => f2 row 50-99
        // f3 dir N   => f2 dir E (0)
        nextR = nextC + 50;
        if (map[nextR, 50] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = 50;
            w.Dir = 0;
            return false;
        }
    }

    // 2->4 4->2
    if (currFace == 2 && nextFace == 4)
    {
        // f2 row  99 => f4 row 100
        // f2 col  xx => f4 row  xx
        // f2 dir S   => f4 dir S (1)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            w.Dir = 1;
            return false;
        }
    }
    if (currFace == 4 && nextFace == 2)
    {
        // f4 row 100 => f2 row  99
        // f4 col  xx => f2 row  xx
        // f4 dir N   => f2 dir N (3)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            w.Dir = 3;
            return false;
        }
    }

    // 3->4 4->3
    if (currFace == 3 && nextFace == 4)
    {
        // f3 row  xx => f4 row  xx
        // f3 col  49 => f4 row  50
        // f3 dir E   => f4 dir E (0)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            w.Dir = 0;
            return false;
        }
    }
    if (currFace == 4 && nextFace == 3)
    {
        // f4 row  xx => f3 row  xx
        // f4 col  50 => f3 row  49
        // f4 dir W   => f3 dir W (2)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            w.Dir = 2;
            return false;
        }
    }

    // 3->5 5->3
    if (currFace == 3 && nextFace == 5)
    {
        // f3 row 149 => f5 row 150
        // f3 col  xx => f5 row  xx
        // f3 dir S   => f5 dir S (1)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            w.Dir = 1;
            return false;
        }
    }
    if (currFace == 5 && nextFace == 3)
    {
        // f5 row 150 => f3 row 149
        // f5 col  xx => f3 row  xx
        // f5 dir N   => f3 dir N (3)
        if (map[nextR, nextC] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = nextC;
            w.Dir = 3;
            return false;
        }
    }

    // 4->5 5->4
    if (currFace == 4 && nextFace == 5)
    {
        // f4 row 149 => f5 col  49
        // f4 col 50-99 => f5 row 150-199
        // f4 dir S   => f5 dir W (2)
        nextR = nextC + 100;
        if (map[nextR, 49] == '#')
            return true;
        else
        {
            w.Row = nextR;
            w.Col = 49;
            w.Dir = 2;
            return false;
        }
    }
    if (currFace == 5 && nextFace == 4)
    {
        // f5 row 150-199 => f4 col 50-99
        // f5 col  49 => f4 row 149
        // f5 dir E   => f4 dir N (3)
        nextC = nextR - 100;
        if (map[149, nextC] == '#')
            return true;
        else
        {
            w.Row = 149;
            w.Col = nextC;
            w.Dir = 3;
            return false;
        }
    }

    // .01
    // .2.
    // 34.
    // 5..
    // face 0 (r = 0-49, c = 50-99)
    // face 1 (r = 0-49, c = 100-149)
    // face 2 (r = 50-99, c = 50-99)
    // face 3 (r = 100-149, c = 0-49)
    // face 4 (r = 100-149, c = 50-99)
    // face 5 (r = 150-199, c = 0-49)

    Console.WriteLine($"*** Invalid edge movement: {currFace} to {nextFace} ***");
    throw new InvalidOperationException();
    return false;
}

void PrintMap()
{
    Console.WriteLine("-----------------");
    for (int i = 0; i < numRows; i++)
    {
        for (int j = 0; j < numCols; j++)
        {
            Console.Write($"{map[i, j]}");
        }
        Console.WriteLine('|');
    }
    Console.WriteLine("-----------------");
}