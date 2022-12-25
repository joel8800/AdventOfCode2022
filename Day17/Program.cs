using Day17;

Console.WriteLine("Day17: Pyroclastic Flow");

string input = File.ReadAllText("input.txt");

long answerPt1 = DropRocks(input, 2022);
long answerPt2 = DropRocks(input, 1000000000000L);

Console.WriteLine($"Part1: {answerPt1}");
Console.WriteLine($"Part2: {answerPt2}");


//=============================================================================

long DropRocks(string input, long maxRocks)
{
    int inputPtr = 0;
    long highPoint = 0;
    long rockCount = 0;
    long patternAdded = 0;

    HashSet<(int x, long y)> column = new() { (0, 0), (1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0) };
    Dictionary<State, (long rockIndex, long top)> states = new();

    while (rockCount < maxRocks)
    {
        long rockType = rockCount % 5;
        Rock r = new(rockType, highPoint + 4);   // gap of 3 between rock and highest point

        while(true)
        {
            // pushed left or right
            if (input[inputPtr] == '<')
                r.MoveLeft(column);
            else
                r.MoveRight(column);

            inputPtr = (inputPtr + 1) % input.Length;

            // then move down
            if (r.MoveDown(column))
            {
                highPoint = column.Select(c => c.y).Max();
                //PrintColumn(column);

                if (highPoint >= 15)
                {
                    var newState = GetState(inputPtr, column, 15);

                    if (states.TryGetValue(newState, out var result))
                    {
                        var distanceY = highPoint - result.top;
                        var numRocks = rockCount - result.rockIndex;
                        var multiple = (maxRocks - rockCount) / numRocks;
                        patternAdded += distanceY * multiple;
                        rockCount += numRocks * multiple;
                        //Console.Write($"found in cache: top:{highPoint} rockIndex:{rockCount} distY:{distanceY} ");
                        //Console.WriteLine($"numRocks:{numRocks} multiple:{multiple} patternAdded:{patternAdded}");
                    }

                    states[newState] = (rockCount, highPoint);
                }
                rockCount += 1;
                break;
            }
        }
    }
    return highPoint + patternAdded;
}

//=============================================================================

State GetState(int inputPtr, HashSet<(int x, long y)> column, long maxHeight)
{
    var maxY = column.Select(c => c.y).Max();
    HashSet<(int, long)> formation = column.Where(c => maxY - c.y < maxHeight).Select(c => (c.x, maxY - c.y)).ToHashSet();
    return new State(inputPtr, formation);
}


void PrintColumn(HashSet<(int x, long y)> column)
{
    long maxY = column.Max(c => c.y);

    for (long y = maxY; y >= 0; y--)
    {
        string row = string.Empty;

        for (int x = 0; x < 7; x++)
        {
            if (column.Contains(new(x, y)))
                row += "#";
            else
                row += ".";
        }
        Console.WriteLine($"|{row}| {y}");
    }
}
