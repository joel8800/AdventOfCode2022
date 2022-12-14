using AoCUtils;
using Day12;

Console.WriteLine("Day12: Hill Climbing Algorithm");

string[] input = FileUtil.ReadFileByLine("input.txt");

int rowSize = input.Length;     //input[0].Length;
int colSize = input[0].Length;   //input.Length;
Console.WriteLine($"rowSize:{rowSize} colSize:{colSize}");

char[,] map = new char[rowSize, colSize];

int rS = 0;
int cS = 0;
int rE = 0;
int cE = 0;

for (int row = 0; row < rowSize; row++)
{
    for (int col = 0; col < colSize; col++)
    {
        char c = input[row][col];
        map[row, col] = c;
        if (c == 'S')
        {
            rS = row;
            cS = col;
            map[row, col] = 'a';
        }
        if (c == 'E')
        {
            rE = row;
            cE = col;
            map[row, col] = 'z';
        }
        //Console.Write(input[row][col]);
    }
    //Console.WriteLine();
}

Queue<GridSquare> q = new();
GridSquare start = new(rS, cS, 0, 'a');
q.Enqueue(start);

HashSet<GridSquare> vis = new();
vis.Add(start);

int[] dRow = { 1, -1, 0, 0 };
int[] dCol = { 0, 0, 1, -1 };

int part1Answer = 0;
while (q.Count > 0)
{
    GridSquare gs = q.Dequeue();

    for (int i = 0; i < 4; i++)
    {
        int nr = gs.X + dRow[i];
        int nc = gs.Y + dCol[i];

        if (nr < 0 || nc < 0 || nr >= rowSize || nc >= colSize)
            continue;

        GridSquare nrnc = new(nr, nc, gs.Distance + 1, map[nr, nc]);
        if (vis.Contains(nrnc))
            continue;

        if (map[nr,nc] - gs.Height > 1)
            continue;

        if (nr == rE && nc == cE)
        {
            part1Answer = gs.Distance + 1;
            Console.WriteLine($"Part1: {gs.Distance + 1}");
            break;
        }

        vis.Add(nrnc);
        q.Enqueue(nrnc);
    }
}


GridSquare end = new(rE, cE, 0, 'z');
q.Clear();
q.Enqueue(end);
vis.Clear();
vis.Add(end);
int part2Answer = 0;
while (q.Count > 0)
{
    GridSquare gs = q.Dequeue();

    for (int i = 0; i < 4; i++)
    {
        int nr = gs.X + dRow[i];
        int nc = gs.Y + dCol[i];

        if (nr < 0 || nc < 0 || nr >= rowSize || nc >= colSize)
            continue;

        GridSquare nrnc = new(nr, nc, gs.Distance + 1, map[nr, nc]);
        if (vis.Contains(nrnc))
            continue;

        if (map[nr, nc] - gs.Height < -1)
            continue;

        if (map[nr, nc] == 'a')
        {
            part2Answer = gs.Distance + 1;
            Console.WriteLine($"Part2: {gs.Distance + 1}");
            break;
        }

        vis.Add(nrnc);
        q.Enqueue(nrnc);
    }
}
