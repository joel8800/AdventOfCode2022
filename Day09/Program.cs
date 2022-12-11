using AoCUtils;
using Day09;

Console.WriteLine("Day 09: Rope Bridge");

string[] inputs = FileUtil.ReadFileByLine("input.txt");

Knot head = new(0, 0);
Knot tail = new(0, 0);

HashSet<Knot> tailPosPt1 = new() { new Knot(0, 0) };

foreach (string inst in inputs)
{
    string[] tokens = inst.Split(' ');
    int steps = Convert.ToInt32(tokens[1]);

    for (int i = 0; i < steps; i++)
    {
        // move head
        head.Move(tokens[0]);

        // add tail position to list if it moved
        if (MoveKnot(head, tail))
        {
            Knot newTail = new(tail.X, tail.Y);
            tailPosPt1.Add(newTail);
    
            //Console.WriteLine($"head{head}  tail{tail} - post move");
        }
    }
}
Console.WriteLine($"Part1: {tailPosPt1.Count}");     // 6081

//-----------------------------------------------------------------------------

List<Knot> rope = new();
for (int i = 0; i < 10; i++)
    rope.Add(new(0, 0));

HashSet<Knot> tailPosPt2 = new() { new Knot(0, 0) };

foreach (string inst in inputs)
{
    string[] tokens = inst.Split(' ');
    string dir = tokens[0];
    int steps = Convert.ToInt32(tokens[1]);

    for (int i = 0; i < steps; i++)
    {
        // move head
        rope[0].Move(tokens[0]);

        // add tail position to list if it moved
        if (MoveRope(rope))
        {
            Knot newTail = new(rope[9].X, rope[9].Y);
            tailPosPt2.Add(newTail);

            //Console.WriteLine($"head{head}  tail{tail} - post move");
        }
    }
}
Console.WriteLine($"Part2: {tailPosPt2.Count}");        // 2487

// ============================================================================

bool MoveRope(List<Knot> rope)
{
    bool moveRequired = false;

    // move each knot in order
    for (int i = 0; i < 9; i++)
    {
        moveRequired = MoveKnot(rope[i], rope[i + 1]);
    }

    return moveRequired;
}

bool MoveKnot(Knot head, Knot tail)
{
    // if knots are in the same position
    if (head.Equals(tail))
        return false;

    // in same row
    if (head.X == tail.X)
    {
        if (Math.Abs(head.Y - tail.Y) > 1)
        {
            // move tail in Y direction
            tail.Y += (head.Y > tail.Y) ? 1 : -1;

            return true;
        }
    }

    // in same column
    if (head.Y == tail.Y)
    {
        if (Math.Abs(head.X - tail.X) > 1)
        {
            // move tail in X direction
            tail.X += head.X > tail.X ? 1 : -1;

            return true;
        }
    }

    // move diagonally if either X or Y difference is > 1
    if ((Math.Abs(head.X - tail.X) > 1) || (Math.Abs(head.Y - tail.Y) > 1))
    {
        // move tail in X direction
        tail.X += head.X > tail.X ? 1 : -1;

        // move tail in Y direction
        tail.Y += head.Y > tail.Y ? 1 : -1;

        return true;
    }

    return false;
}
