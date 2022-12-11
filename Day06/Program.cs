Console.WriteLine("Day06: Tuning Trouble");

string input = File.ReadAllText("input.txt");

int indexPt1 = 0;
for (int i = 3; i < input.Length; i++)
{
    HashSet<char> chars = new();

    for (int j = 3; j >= 0; j--)
        chars.Add(input[i - j]);

    if (chars.Count == 4)
    {
        indexPt1 = i + 1;
        break;
    }

    chars.Clear();
}

Console.WriteLine($"Part1: {indexPt1}");

int indexPt2 = 0;
for (int i = 13; i < input.Length; i++)
{
    HashSet<char> chars = new();

    for (int j = 13; j >= 0; j--)
        chars.Add(input[i - j]);

    if (chars.Count == 14)
    {
        indexPt2 = i + 1;
        break;
    }

    chars.Clear();
}

Console.WriteLine($"Part2: {indexPt2}");
