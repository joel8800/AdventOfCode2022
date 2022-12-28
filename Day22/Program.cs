using AoCUtils;
using Day22;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;

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

PrintMap();

Regex re = new(@"\d+|[RL]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

MatchCollection matches = re.Matches(input[1]);
List<string> commands = matches.Select(x => x.Value).ToList();
Console.WriteLine($"matches found:{matches.Count}");
Console.WriteLine($"commands     :{commands.Count}");

// find start
Walker w = new(0, 0, 0);
while (map[1, w.Col] == ' ')
    w.Col++;

Console.WriteLine($"at [{w.Row},{w.Col}] {map[w.Row, w.Col]} facing {w.OrdDir()}");

int deltaR = 0;
int deltaC = 1;
foreach (string command in commands)
{
    if (int.TryParse(command, out int steps))
    {
        Console.WriteLine($"-- move {steps}");
        //Move(w, steps);
        for (int i = 0; i < steps; i++)
        {
            int nextR = w.Row;
            int nextC = w.Col;

            while(true)
            {
                nextR = (nextR + deltaR) % numRows;
                nextC = (nextC + deltaC) % numCols;

                if (nextR >= numRows)
                    nextR = 0;
                if (nextR < 0)
                    nextR = numRows - 1;

                if (nextC >= numCols)
                    nextC = 0;
                if (nextC < 0)
                    nextC = numCols - 1;
                
                if (map[nextR, nextC] != ' ')           // nextR became -1 here?
                    break;
            }
            if (map[nextR, nextC] == '#')
                break;

            w.Row = nextR;
            w.Col = nextC;
            Console.WriteLine($"at [{w.Row},{w.Col}] {map[w.Row, w.Col]} facing {w.OrdDir()}");
        }
    }
    else
    {
        Console.WriteLine($"-- turn {command}");
        //Turn(w, command);
        if (command == "R")
        {
            int tmp = deltaR;

            deltaR = deltaC;
            deltaC = -tmp;
        }
        else
        {
            int tmp = deltaR;

            deltaR = -deltaC;
            deltaC = tmp;
        }
    }
    Console.WriteLine($"at [{w.Row},{w.Col}] {map[w.Row, w.Col]} facing {w.OrdDir()}");
}


int answerPt1 = 1000 * (w.Row + 1) + 4 * (w.Col + 1) + w.Dir;
Console.WriteLine($"Part1: {answerPt1}");

//-----------------------------------------------------------------------------

//=============================================================================



void MoveHorizontally(Walker w, int steps)
{
    int direction;
    int first;
    int last;

    if (w.Dir == 0)
    {
        direction = 1;
        first = 0;
        last = numCols - 1;
    }
    else
    {
        direction = -1;
        first = numCols - 1;
        last = 0;
    }

    for (int i = 0; i < steps; i++)
    {
        Console.WriteLine($"stepping to col {w.Col + direction}");
        if (w.Col + direction == last)       // check inbounds, wrap if needed
        {
            Console.WriteLine("reached map edge");
            int col = first;

            while (map[w.Row, col] == ' ')
                col += direction;

            if (map[w.Row, col] == '#')
            {
                Console.WriteLine("hit a wall");
                return;
            }
            else
                w.Col = col;

            Console.WriteLine($"col = {col}");
            continue;
        }

        if (map[w.Row, w.Col + 1] == ' ')       // hit space, wrap
        {
            Console.WriteLine("reached visible edge");
            int col = first;

            while (map[w.Row, col] == ' ')
                col += direction;

            if (map[w.Row, col] == '#')
            {
                Console.WriteLine("hit a wall");
                return;
            }
            else
                w.Col = col;

            Console.WriteLine($"col = {col}");
            continue;
        }

        if (map[w.Row, w.Col + direction] == '#')
        {
            Console.WriteLine("hit a wall");
            return;
        }

        if (map[w.Row, w.Col + direction] == '.')
            w.Col += direction;
    }
}


void MoveVertically(Walker w, int steps)
{
    int direction;
    int first;
    int last;

    if (w.Dir == 1)
    {
        direction = 1;
        first = 0;
        last = numRows - 1;
    }
    else
    {
        direction = -1;
        first = numRows - 1;
        last = 0;
    }

    for (int i = 0; i < steps; i++)
    {
        Console.WriteLine($"stepping to row {w.Row + direction}");
        if (w.Row + direction < last || w.Row + direction > first)          // check inbounds, wrap if needed
        {
            Console.WriteLine("reached map edge");
            int row = first;

            while (map[row, w.Col] == ' ')
                row += direction;

            if (map[row, w.Col] == '#')
            {
                Console.WriteLine("hit a wall");
                return;
            }
            else
                w.Row = row;

            Console.WriteLine($"row = {row}");
            continue;
        }

        if (map[w.Row + direction, w.Col] == ' ')
        {
            Console.WriteLine("reached visible edge");
            int row = first;

            while (map[row, w.Col] == ' ')
                row += direction;

            if (map[row, w.Col] == '#')
            {
                Console.WriteLine("hit a wall");
                return;
            }
            else
                w.Row = row;

            Console.WriteLine($"row = {row}");
            continue;
        }

        if (map[w.Row + direction, w.Col] == '#')
        {
            Console.WriteLine("hit a wall");
            return;
        }

        if (map[w.Row + direction, w.Col] == '.')
            w.Row += direction;
    }
}

void Move(Walker w, int steps)
{
    Console.WriteLine($"-- moving {steps} steps in direction {w.Dir}");
    if (w.Dir == 0 || w.Dir == 2)
        MoveHorizontally(w, steps);

    if (w.Dir == 1 || w.Dir == 3)
        MoveVertically(w, steps);

    Console.WriteLine($"now at [{w.Row},{w.Col}] ({map[w.Row, w.Col]}) facing {w.Dir}");
}

void Turn(Walker w, string direction)
{
    Console.Write($"-- turning {direction} from {w.Dir} to ");
    if (direction == "R")
        w.Dir += 1;
    else
        w.Dir -= 1;
    w.Dir = (w.Dir + 4) % 4;
    Console.WriteLine(w.Dir);
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