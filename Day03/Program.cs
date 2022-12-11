Console.WriteLine("Day03: Rucksack Reorganization");

string[] input = File.ReadAllLines("input.txt");

int sumPt1 = 0;
foreach (string line in input)
{
    int length = line.Length;
    string compart1 = line.Substring(0, length / 2);
    string compart2 = line.Substring(length / 2, length / 2);

    List<char> items1 = compart1.ToList();
    List<char> items2 = compart2.ToList();

    char common = items1.Intersect(items2).First();

    int priority = ItemToPriority(common);
    //Console.WriteLine($"intersection {common} {priority}");
    
    sumPt1 += priority;
}

Console.WriteLine($"Part1: {sumPt1}");      // 7872

int sumPt2 = 0;
for (int i = 0; i < input.Length; i += 3)
{
    List<char> elf1 = input[i + 0].ToList();
    List<char> elf2 = input[i + 1].ToList();
    List<char> elf3 = input[i + 2].ToList();

    char badge = elf1.Intersect(elf2).Intersect(elf3).First();

    int priority = ItemToPriority(badge);
    //Console.WriteLine($"intersection {badge} {priority}");

    sumPt2 += priority;
}

Console.WriteLine($"Part2: {sumPt2}");      // 2497

//-----------------------------------------------------------------------------

int ItemToPriority(char c)
{
    if (char.IsLower(c))
        return c - 'a' + 1;
    else
        return c - 'A' + 27;
}
