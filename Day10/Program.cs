using AoCUtils;
using System.Text;

Console.WriteLine("Day10:");

string[] inst = FileUtil.ReadFileByLine("input.txt");

int X = 1;
int cycle = 1;

List<int> sigStrengths = new List<int>();
int checkCycle = 20;

for (int i = 0; i < inst.Length; i++)
{
    //Console.WriteLine($"X: {X}   cycle: {cycle}");
    string[] tokens = inst[i].Split(' ');

    if (inst[i].StartsWith("noop"))
    {
        if (cycle == checkCycle)
        {
            sigStrengths.Add(X * cycle); 
            checkCycle += 40;
        }
        cycle++;
        continue;
    }

    if (inst[i].StartsWith("addx"))
    {
        int addend = Convert.ToInt32(tokens[1]);

        if (cycle == checkCycle)
        {
            sigStrengths.Add(X * cycle);
            checkCycle += 40;
        }
        cycle++;

        if (cycle == checkCycle)
        {
            sigStrengths.Add(X * cycle);
            checkCycle += 40;
        }
        cycle++;

        X += addend;
        continue;
    }
}

int sumStrengths = sigStrengths.Sum();
Console.WriteLine($"Part1: {sumStrengths}");


X = 1;
cycle = 1;
StringBuilder crt = new StringBuilder();

for (int i = 0; i < inst.Length; i++)
{
    //Console.WriteLine($"X: {X}   cycle: {cycle}");
    int cyc;
    string[] tokens = inst[i].Split(' ');

    if (inst[i].StartsWith("noop"))
    {
        cyc = cycle % 40;
        if ((cyc == X) || (cyc == X + 1) || (cyc == X + 2))
            crt.Append('#');
        else
            crt.Append('.');
        cycle++;
        continue;
    }

    if (inst[i].StartsWith("addx"))
    {
        int addend = Convert.ToInt32(tokens[1]);

        // 1st cycle
        cyc = cycle % 40;
        if ((cyc == X) || (cyc == X + 1) || (cyc == X + 2))
            crt.Append('#');
        else
            crt.Append('.');
        cycle++;

        // 2nd cycle
        cyc = cycle % 40;
        if ((cyc == X) || (cyc == X + 1) || (cyc == X + 2))
            crt.Append('#');
        else
            crt.Append('.');
        cycle++;
        X += addend;

        continue;
    }
}

string crt240 = crt.ToString().Substring(0, 240);
string crt0 = crt240.Substring(0, 40);
string crt1 = crt240.Substring(40, 40);
string crt2 = crt240.Substring(80, 40);
string crt3 = crt240.Substring(120, 40);
string crt4 = crt240.Substring(160, 40);
string crt5 = crt240.Substring(200, 40);

Console.WriteLine("Part2:");
Console.WriteLine(crt0);
Console.WriteLine(crt1);
Console.WriteLine(crt2);
Console.WriteLine(crt3);
Console.WriteLine(crt4);
Console.WriteLine(crt5);
