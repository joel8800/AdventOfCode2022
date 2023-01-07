using AoCUtils;
using Day19;
using System.Text.RegularExpressions;

// Solution converted from hyper-neutrino's python code. Huge learning experience for me.

Console.WriteLine("Day19: Not Enough Minerals");

string[] input = FileUtil.ReadFileByLine("inputSamp.txt");  // Part1: 817   Part2: 4216

List<Blueprint> bots = new();

foreach (string line in input)
{
    Blueprint bp = new(line);
    bots.Add(bp);
    Console.WriteLine(bp);
}


int answerPt1 = 0;

foreach (var bot in bots)
{
    int qLevel = bot.GetQualityLevel();
    
    Console.WriteLine($"{bot.ID}: quality:{qLevel}");
    answerPt1 += qLevel;
}

Console.WriteLine($"Part1: {answerPt1}");

