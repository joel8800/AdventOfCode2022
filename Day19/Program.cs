using AoCUtils;
using Day19;

// Solution converted from hyper-neutrino's python code. Huge learning experience for me.
Console.WriteLine("Day19: Not Enough Minerals");

string[] input = FileUtil.ReadFileByLine("input.txt");  // Part1: 817   Part2: 4216

int answerPt1 = 0;
foreach (string line in input)
{
    Blueprint bp = new(line);

    int qLevel = bp.GetQualityLevel(24);
    answerPt1 += qLevel;
}
Console.WriteLine($"Part1: {answerPt1}");

int answerPt2 = 1;
for (int i = 0; i < 3; i++)
{
    Blueprint bp = new(input[i]);

    int maxGeodes = bp.GetMaximumGeodes(32);
    answerPt2 *= maxGeodes;
}
Console.WriteLine($"Part2: {answerPt2}");

