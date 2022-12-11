using AoCUtils;

Console.WriteLine("Day04: Camp Cleanup");

string[] input = FileUtil.ReadFileByLine("input.txt");

int containing = 0;
int overlapping = 0;

for (int i = 0; i < input.Length; i++)
{
    string[] elfPair = input[i].Split(',');
    
    List<string> elf1 = elfPair[0].Split('-').ToList();
    List<string> elf2 = elfPair[1].Split('-').ToList();

    HashSet<int> elf1Sections = GetHashSet(elf1);
    HashSet<int> elf2Sections = GetHashSet(elf2);
    
    if (Contains(elf1Sections, elf2Sections))
        containing++;

    if (Overlaps(elf1Sections, elf2Sections))
        overlapping++;
}

Console.WriteLine($"Part1: {containing}");
Console.WriteLine($"Part2: {overlapping}");

// ----------------------------------------------------------

HashSet<int> GetHashSet(List<string> elf)
{
    HashSet<int> set = new();

    int min = int.Parse(elf[0]);
    int max = int.Parse(elf[1]);

    for (int i = min; i <= max; i++)
        set.Add(i);

    return set;
}

bool Contains(HashSet<int> set1, HashSet<int> set2)
{
    HashSet<int> intersect12 = new(set1);
    HashSet<int> intersect21 = new(set2);

    intersect12.IntersectWith(set2);
    intersect21.IntersectWith(set1);

    if ((intersect12.Count == set1.Count) || (intersect21.Count == set2.Count))
        return true;
    else
        return false;
}

bool Overlaps(HashSet<int> set1, HashSet<int> set2)
{
    HashSet<int> intersect12 = new(set1);

    intersect12.IntersectWith(set2);

    if (intersect12.Count > 0)
        return true;
    else
        return false;
}