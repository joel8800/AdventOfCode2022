using AoCUtils;
using Day19;
using System.Text.RegularExpressions;

Console.WriteLine("Day19: Not Enough Minerals");

string[] input = FileUtil.ReadFileByLine("inputSamp.txt");

List<Blueprint> bots = new();

foreach (string line in input)
{
    MatchCollection mc = Regex.Matches(line, @"\d+");
    int id = Convert.ToInt32(mc[0].Value);
    int oreCost = Convert.ToInt32(mc[1].Value);
    int clayCost = Convert.ToInt32(mc[2].Value);
    int obsCostOre = Convert.ToInt32(mc[3].Value);
    int obsCostCly = Convert.ToInt32(mc[4].Value);
    int geoCostOre = Convert.ToInt32(mc[5].Value);
    int geoCostObs = Convert.ToInt32(mc[6].Value);


    Blueprint bp = new(id, oreCost, clayCost, obsCostOre, obsCostCly, geoCostOre, geoCostObs);
    bots.Add(bp);
}

for (int i = 0; i < 24; i++)
{
    foreach (var bot in bots)
    {
        bot.NextMinute();
        Console.WriteLine($"{i + 1}: {bot}");
    }
    Console.WriteLine();
}

foreach (var bot in bots)
{
    Console.WriteLine($"{bot.ID}: quality:{bot.GetQualityLevel()}");
}

Console.WriteLine($"Part1: ");

