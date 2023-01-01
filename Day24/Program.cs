using AoCUtils;

Console.WriteLine("Day24: Blizzard Basin");

bool verbose = true;

string[] input = FileUtil.ReadFileByLine("inputSamp.txt");  // part1: 292  part2: 816
int numRows = input.Length;
int numCols = input[0].Length;


string arrows = ">v<^";
Dictionary<int, (int r, int c)> moves = new()
{
    { 0, (0, 1) },      // > move right
    { 1, (1, 0) },      // v move down
    { 2, (0, -1) },     // < move left
    { 3, (-1, 0) },     // ^ move up
    { 4, (0, 0) }       // o stay put
};

HashSet<(int r , int c, int d)> blizzards = new();
for (int i = 0; i < numRows; i++)
    for (int j = 0; j < numCols; j++)
        if (arrows.Contains(input[i][j]))
            blizzards.Add((i, j, arrows.IndexOf(input[i][j])));

PrintMap(blizzards, numRows, numCols, arrows);

// since blizzards cycle repeatably, there are a limited number
// of states of blizzard locations
int period = MathUtil.LCM(input[0].Length - 2, input.Length - 2);

// create list of all possible states of blizzard locations
List<HashSet<(int r, int c, int d)>> blizzardStates = new()
{
    blizzards
};

LogPrintLn("calculating blizzard states");
// calculate positions of blizzards for each state and add to list
for (int i = 1; i < period; i++)
{
    HashSet<(int r, int c, int d)> nextBlizzard = new();

    foreach (var b in blizzards)
    {
        int nextRow = b.r + moves[b.d].Item1;
        int nextCol = b.c + moves[b.d].Item2;

        if (nextRow == 0)
            nextRow = numRows - 2;
        else if (nextRow == numRows - 1)
            nextRow = 1;

        if (nextCol == 0)
            nextCol = numCols - 2;
        else if (nextCol == numCols - 1)
            nextCol = 1;

        nextBlizzard.Add((nextRow, nextCol, b.d));
    }

    blizzardStates.Add(nextBlizzard);
    blizzards = nextBlizzard;

    LogPrintLn($"-- state {i}");
    //PrintMap(blizzards, numRows, numCols, arrows);
}
LogPrintLn("blizzard states calculated");

var start = (0, 1);
var end = (numRows - 1, numCols - 2);
LogPrintLn($"start({start}), end({end})");

HashSet <((int r, int c), int t)> visited = new();
PriorityQueue<((int r, int c) rc, int t), int> q= new();

q.Enqueue((start, 0), 0);

int answerPt1 = 0;

while (q.Count > 0)
{
    LogPrintLn($"q count: {q.Count}");

    var curr = q.Dequeue();
    LogPrint($"Dequeued ({curr.t}, ({curr.Item1.r}, {curr.Item1.c}) - ");
    
    if (visited.Contains(curr))
    {
        LogPrintLn("already in visited");
        continue;
    }

    LogPrintLn("adding to visited");
    visited.Add(curr);
    
    int time = curr.t;
    int currRow = curr.rc.r;
    int currCol = curr.rc.c;

    // did we reach the end?
    if (curr.rc == end)
    {
        LogPrintLn("reached end");
        answerPt1 = time;
        break;
    }
    // check neighbors
    for (int i = 0; i < 5; i++)
    {
        LogPrint($"-- check neighbor {i} of ({time}, ({currRow},{currCol})) ");
        // calc next move
        int nextRow = currRow + moves[i].r;
        int nextCol = currCol + moves[i].c;

        // discard if next move is out of bounds or start/end positions
        if (!((nextRow, nextCol) == start) && 
            !((nextRow, nextCol) == end) &&
            !(IsInBounds(nextRow, nextCol, numRows, numCols)))
        {
            LogPrintLn($"---- ({time + 1}, (({nextRow},{nextCol})) out of bounds");
                continue;
        }

        // check if collision with blizzard
        if (IsOccupied(nextRow, nextCol, blizzardStates[(time + 1) % period]))
        {
            LogPrintLn($"---- ({time + 1}, ({nextRow},{nextCol})) will be occupied at t = {time + 1}");
            continue;
        }

        // queue next position if we get here
        q.Enqueue(((nextRow, nextCol), time + 1), time + 1);
        LogPrintLn($"-- adding ({time + 1}, ({nextRow},{nextCol})) to queue)");
    }
}

Console.WriteLine($"Part1: {answerPt1}");


//=============================================================================

bool IsOccupied(int row, int col, HashSet<(int r, int c, int d)> blizzards)
{
    // check if any blizzard matches
    for (int d = 0; d < 4; d++)
    {
        if (blizzards.Contains((row, col, d)))
            return true;
    }
    return false;
}

bool IsInBounds(int row, int col, int numRows, int numCols)
{
    // row/col 0 is OOB, last row/col is OOB
    if (row > 0 && row < numRows - 1 && col > 0 && col < numCols - 1)
        return true;
    else
        return false;
}

void LogPrintLn(string msg)
{
    if (verbose)
        Console.WriteLine(msg);
}

void LogPrint(string msg)
{
    if (verbose)
        Console.Write(msg);
}

void PrintMap(HashSet<(int r, int c, int d)> blizzards, int numRows, int numCols, string arrows)
{
    char[,] map = new char[numCols, numRows];
    for (int r = 0; r < numRows; r++)
        for (int c = 0; c < numCols; c++)
            map[c, r] = '.';

    foreach (var b in blizzards)
    {
        int row = b.r;
        int col = b.c;
        char c = arrows[b.d];
        map[col, row] = c;
    }

    for (int r = 0; r < numRows; r++)
    {
        for (int c = 0; c < numCols; c++)
        {
            LogPrint($"{map[c, r]}"); 
        }
        LogPrintLn("");
    }
    LogPrintLn("");
}


