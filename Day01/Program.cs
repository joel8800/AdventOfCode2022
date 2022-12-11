using AoCUtils;

Console.WriteLine("Day01: Calorie Counting");

string[] elfCals = FileUtil.ReadFileByBlock("input.txt");

List<int> elves = new();

foreach (string cals in elfCals)
{
    string[] calVals = cals.Split(Environment.NewLine);

    int totalCals = 0;
    foreach (string cal in calVals)
    {
        totalCals += int.Parse(cal);
    }
    elves.Add(totalCals);
}

int maxCals = elves.Max();
Console.WriteLine($"Part1: {maxCals} calories");

int top3 = maxCals;

// get 2nd most calories
elves.Remove(maxCals);
maxCals = elves.Max();
top3 += maxCals;

// get 3rd most calories
elves.Remove(maxCals);
maxCals = elves.Max();
top3 += maxCals;

Console.WriteLine($"Part2: {top3} calories");
