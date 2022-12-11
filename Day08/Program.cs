using AoCUtils;

Console.WriteLine("Day08: Treetop Tree House");

List<List<int>> forest = FileUtil.ReadFileToIntGrid("input.txt");

int rowSize = forest[0].Count;
int colSize = forest.Count;

int visible = (rowSize * 2) + (colSize * 2) - 4;
for (int y = 1; y < colSize - 1; y++)
{
    for (int x = 1; x < rowSize - 1; x++)
    {
        if (IsVisible(forest, x, y))
            visible++;
    }
}
Console.WriteLine($"Part1: {visible}");

int mostScenic = 0;
for (int y = 1; y < colSize - 1; y++)
{
    for (int x = 1; x < rowSize - 1; x++)
    {
        int score = ScenicScore(forest, x, y);
        if (score > mostScenic)
            mostScenic = score;
    }
}
Console.WriteLine($"Part2: {mostScenic}");


//===================================================================

bool IsVisible(List<List<int>> forest, int x, int y)
{
    int treeHeight = forest[x][y];
    bool fromWest = true;
    bool fromEast = true;
    bool fromNorth = true;
    bool fromSouth = true;

    //Console.Write($"Checking [{x},{y}] height : {treeHeight}");

    // look from west
    for (int i = x - 1; i >= 0; i--)
    {
        if (treeHeight <= forest[i][y])
        {
            fromWest = false;
            break;
        }
    }

    // look from east
    for (int i = x + 1; i <= rowSize - 1; i++)
    {
        if (treeHeight <= forest[i][y])
        {
            fromEast = false;
            break;
        }
    }

    // look from north
    for (int i = y - 1; i >= 0; i--)
    {
        if (treeHeight <= forest[x][i])
        {
            fromNorth = false;
            break;
        }
    }

    // look from south
    for (int i = y + 1; i <= colSize - 1; i++)
    {
        if (treeHeight <= forest[x][i])
        {
            fromSouth = false;
            break;
        }
    }

    bool visible = fromWest || fromEast || fromNorth || fromSouth;
    //Console.WriteLine($" {visible}");
    return visible;
}

int ScenicScore(List<List<int>> forest, int x, int y)
{
    int treeHeight = forest[x][y];
    int west = 0;
    int east = 0;
    int north = 0;
    int south = 0;

    //Console.Write($"Checking [{x},{y}] height {treeHeight}: ");

    // look from west
    for (int i = x - 1; i >= 0; i--)
    {
        west++;
        if (treeHeight <= forest[i][y])
            break;
    }

    // look from east
    for (int i = x + 1; i <= rowSize - 1; i++)
    {
        east++;
        if (treeHeight <= forest[i][y])
            break;
    }

    // look from north
    for (int i = y - 1; i >= 0; i--)
    {
        north++;
        if (treeHeight <= forest[x][i])
            break;
    }

    // look from south
    for (int i = y + 1; i <= colSize - 1; i++)
    {
        south++;
        if (treeHeight <= forest[x][i])
            break;
    }

    int score = west * east * north * south;
    //Console.WriteLine($": {score}");
    return score;

}