using AoCUtils;
using Day23;

Console.WriteLine("Day23: Unstable Diffusion");

Dictionary<int, List<int>> offsets = new()
{
    { 0, new List<int> { 0xd, 0x1, 0x5 } },         // NW, N, NE
    { 1, new List<int> { 0x7, 0x3, 0xf } },         // SE, S, SW
    { 2, new List<int> { 0xf, 0xc, 0xd } },         // SW, W, NW
    { 3, new List<int> { 0x5, 0x4, 0x7 } },         // NE, E, SE
    { 8, new List<int> { 0x1, 0x5, 0x4, 0x7, 0x3, 0xf, 0xc, 0xd } }
};


string[] input = FileUtil.ReadFileByLine("input.txt");  // part1: 3987  part2: 938

int xMax = input[0].Length;
int yMax = input.Length;

List<Elf> elves = new();

for (int y = 0; y < yMax; y++)
{
    for (int x = 0; x < xMax; x++)
    {
        if (input[y][x] == '#')
            elves.Add(new(x, yMax - y));
    }
}

PrintElfGrid(elves);
int direction = 0;      // 0=N, 1=S, 2=W, 3=E
int answerPt1 = 0;
int answerPt2 = 0;

for (int i = 0; i < 1000; i++)
{
    int movesThisRound = 0;

    // first half of round: check around and propose
    foreach (Elf e in elves)
    {
        e.Proposed = false;

        //Console.WriteLine($"[{e.X},{e.Y}] =====================");
        if (IsClearAround(elves, e) == false)
        {
            e.WantsToMove = true;

            if (ProposeMoves(elves, e, direction) == true)
            {
                //Console.WriteLine($"[{e.X},{e.Y}]: is able to move");
                e.WantsToMove = true;
            }
            else
            {
                //Console.WriteLine($"[{e.X},{e.Y}]: is blocked in");
                e.WantsToMove = false;
            }
        }
        else
        {
            //Console.WriteLine($"[{e.X},{e.Y}]: area clear, no need to move");
            e.WantsToMove = false;
        }
    }

    //PrintProposals(elves);

    // second half of round: check for conflicts
    foreach (Elf e in elves)
    {
        if (e.WantsToMove)
        {
            // check if anyone else wants proposed tile
            var others = elves.Where(el => el.Xp == e.Xp).Where(el => el.Yp == e.Yp).Where(el => el.Proposed == true);
            foreach (Elf e2 in others)
            {
                if (e != e2)
                {
                    //Console.WriteLine($"[{e.X},{e.Y}] and [{e2.X},{e2.Y}] want [{e.Xp},{e.Yp}]");
                    e.WantsToMove = false;
                    e2.WantsToMove = false;
                }
            }
        }
    }

    // move elves
    foreach (Elf e in elves)
    {
        if (e.WantsToMove)
        {
            e.Move();
            movesThisRound += 1;
        }
    }

    Console.WriteLine($"=== End of round {i + 1} === {movesThisRound} moves");
    if (movesThisRound == 0)
    {
        answerPt2 = i + 1;
        break;
    }
    if (i == (10 - 1))
        answerPt1 = CalcAnswerPt1(elves);

    //PrintElfGrid(elves);
    direction = (direction + 1) % 4;
}

PrintElfGrid(elves);

Console.WriteLine($"Part1: {answerPt1}");
Console.WriteLine($"Part2: {answerPt2}");

//=============================================================================

bool ProposeMoves(List<Elf> elves, Elf me, int initialDir)
{
    int dir = initialDir;

    for (int i = 0; i < 4; i++)
    {
        dir = (initialDir + i) % 4;
        if (IsDirectionOpen(elves, me, dir))
        {
            if (me.Proposed == false)
            {
                me.ProposeMove(dir);
                //Console.WriteLine($"-- direction {dir} is open - proposing [{me.X},{me.Y}] to [{me.Xp},{me.Yp}]");
                return true;
            }
            else
            {
                //Console.WriteLine("-- proposal already made");
            }
        }
        //else
        //    Console.WriteLine($"-- direction {dir} is blocked");
    }

    return false;
}

bool IsDirectionOpen(List<Elf> elves, Elf me, int direction)
{
    //Console.WriteLine($"- checking direction {direction}");
    foreach (int offset in offsets[direction])
    {
        if (IsOccupied(elves, me, offset))
            return false;
    }

    return true;
}

bool IsClearAround(List<Elf> elves, Elf me)
{
    //Console.WriteLine("- checking surrounding tiles");
    foreach (int offset in offsets[8])
    {
        if (IsOccupied(elves, me, offset))
            return false;
    }

    return true;
}

int PosOffsetX(int pos)
{
    int bit = (pos >> 2) & 0x1;
    bit = (pos & 0x8) > 1 ? -bit : bit;

    return bit;
}

int PosOffsetY(int pos)
{
    int bit = pos & 0x1;
    bit = (pos & 0x2) > 1 ? -bit : bit;

    return bit;
}

bool IsOccupied(List<Elf> elves, Elf me, int pos)
{
    int x = me.X + PosOffsetX(pos);
    int y = me.Y + PosOffsetY(pos);

    //Console.WriteLine($"-- checking me[{me.X},{me.Y}] vs [{x},{y}]");
    if (elves.Where(e => e.X == x).Any(e => e.Y == y))
        return true;
    else
        return false;
}

void PrintElfGrid(List<Elf> elves)
{
    int yMax = elves.Select(e => e.Y).Max();
    int yMin = elves.Select(e => e.Y).Min();
    int xMax = elves.Select(e => e.X).Max();
    int xMin = elves.Select(e => e.X).Min();

    for (int y = yMax; y >= yMin; y--)
    {
        for (int x = xMin; x <= xMax; x++)
        {
            if (elves.Where(e => e.X == x).Any(e => e.Y == y))
                Console.Write('#');
            else if (x ==0 && y == 0)
                Console.Write('O');
            else
                Console.Write('.');
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

void PrintProposals(List<Elf> elves)
{
    foreach (Elf e in elves)
    {
        if (e.Proposed)
            Console.WriteLine($"[{e.X},{e.Y}] proposes [{e.Xp},{e.Yp}]: {e.Proposed}, wants to move: {e.WantsToMove} ");
        else
            Console.WriteLine($"[{e.X},{e.Y}] did not propose a move");
    }
}

int CalcAnswerPt1(List<Elf> elves)
{
    int xMax = elves.Select(e => e.X).Max();
    int xMin = elves.Select(e => e.X).Min();
    int yMax = elves.Select(e => e.Y).Max();
    int yMin = elves.Select(e => e.Y).Min();

    int x = xMax - xMin + 1;
    int y = yMax - yMin + 1;

    return (x * y) - elves.Count();
}