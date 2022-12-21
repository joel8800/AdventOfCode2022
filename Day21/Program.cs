using AoCUtils;
using Day21;

Console.WriteLine("Day21: ");

string[] input = FileUtil.ReadFileByLine("input.txt");  // part1: 51928383302238  part2: 3305669217840

List<Monkey> monkeys = ReadInput(input);

int iterations = 0;
while(Resolve(monkeys) == false)
{
    iterations++;
    if (iterations > 50)    // less than 50 iterations to resolve all monkeys
        break;
    //Console.WriteLine($"===== iteration {iterations} =====");
}

Monkey rootPt1 = monkeys.Single(mx => mx.Name == "root");
Console.WriteLine($"Part1: {rootPt1.Value}");

//-----------------------------------------------------------------------------
// root: jhpn + jmsg  <-- from input
// jhpn decreases as humn increases
// binary search humn until value of jhpn == value of jmsg

List<long> trials = new() { 1000000, 10000000, 100000000, 1000000000, 
    10000000000, 100000000000, 1000000000000, 5000000000000 };

Console.WriteLine($"left:jhpn              right:jmsg");
foreach (long trial in trials)
{
    monkeys.Clear();
    monkeys = ReadInput(input);

    Monkey humn = monkeys.Single(mx => mx.Name == "humn");
    humn.Value = trial;

    long score = ResolveLoop(monkeys);
    
    //iterations = 0;
    //while (Resolve(monkeys) == false)
    //{
    //    iterations++;
    //    if (iterations > 50)
    //        break;
    //}

    Monkey root = monkeys.Single(mx => mx.Name == "root");
    Monkey left = monkeys.Single(mx => mx.Name == "jhpn");
    Monkey right = monkeys.Single(mx => mx.Name == "jmsg");
    Console.WriteLine($"{left.Value}         {right.Value}");
}

// binary search
long numLo = 1000000000000;
long numHi = 5000000000000;
long testPoint = 0;

while (numLo < numHi)
{
    testPoint = (numLo + numHi) / 2;
    //Console.WriteLine($"low:{numLo}  mid:{testPoint}  high:{numHi}");
    Console.WriteLine($"test: {testPoint}");

    monkeys.Clear();
    monkeys = ReadInput(input);

    Monkey humn = monkeys.Single(mx => mx.Name == "humn");
    humn.Value = testPoint;

    long result = ResolveLoop(monkeys);

    if (result == 0)
        break;

    if (result < 0)
        numLo = testPoint;
    else
        numHi = testPoint;
}

Console.WriteLine();
Console.WriteLine($"Part2: {testPoint}");
Console.WriteLine();


for (long i = -5; i <= 8; i++)
{
    monkeys.Clear();
    monkeys = ReadInput(input);

    Monkey human = monkeys.Single(mx => mx.Name == "humn");
    human.Value = testPoint + i;

    long postTest = ResolveLoop(monkeys);
    Console.WriteLine($"testPoint:{testPoint + i}  result:{postTest}");
}
// rounding error?  multiple values result in 0
//testPoint: 3305669217838  result: -27
//testPoint: 3305669217839  result: -27
//testPoint: 3305669217840  result: 0   <-- this answer accepted by AoC
//testPoint: 3305669217841  result: 0
//testPoint: 3305669217842  result: 0
//testPoint: 3305669217843  result: 0
//testPoint: 3305669217844  result: 0
//testPoint: 3305669217845  result: 0
//testPoint: 3305669217846  result: 23
//testPoint: 3305669217847  result: 23
//=============================================================================

long ResolveLoop(List<Monkey> monkeys)
{
    iterations = 0;
    while (Resolve(monkeys) == false)
    {
        iterations++;
        if (iterations > 50)
            break;
    }
    Monkey jhpn = monkeys.Single(mx => mx.Name == "jhpn");
    Monkey jmsg = monkeys.Single(mx => mx.Name == "jmsg");

    long score = jmsg.Value - jhpn.Value;
    //Console.WriteLine($"jhpn:{jhpn.Value}  jmsg:{jmsg.Value}  score:{score}  ");
    return score;
}



bool Resolve(List<Monkey> monkeys)
{
    //Console.WriteLine("resolving...");

    int resolveCount = 0;
    foreach (Monkey m in monkeys)
    {
        //Console.WriteLine($"checking {m.Name} - {m.Value}");
        if (m.Value > 0 || m.Name == "humn")
            continue;

        Monkey lm = monkeys.Single(mx => mx.Name == m.lName);
        Monkey rm = monkeys.Single(mx => mx.Name == m.rName);

        //Console.Write("checking left and right monkeys: ");
        //Console.WriteLine($"{lm.Name}:{lm.Value}  {rm.Name}:{rm.Value}");
        long result = 0;
        if (lm.Value > 0 && rm.Value > 0)
        {
            //Console.WriteLine($"-- resolving {m.Name}: {lm.Name}({lm.Value}) {m.op} {rm.Name}({rm.Value})");
            switch (m.op)
            {
                case "+": result = lm.Value + rm.Value; break;
                case "-": result = lm.Value - rm.Value; break;
                case "*": result = lm.Value * rm.Value; break;
                case "/": result = lm.Value / rm.Value; break;
                case "=": result = lm.Value == rm.Value ? 1 : 0; break;
                default: Console.WriteLine("*** invalid operator"); break;
            }

            m.Value = result;
            resolveCount++;
        }
    }

    //Console.WriteLine($"----- resolved {resolveCount} monkeys");
    if (resolveCount > 0)
        return false;   // not done
    else
        return true;    // done
}


List<Monkey> ReadInput(string[] input)
{
    List<Monkey> monkeys = new();

    for (int i = 0; i < input.Length; i++)
    {
        string[] monkeyInput = input[i].Split(": ");

        Monkey newMonkey;

        if (long.TryParse(monkeyInput[1], out long value))
            newMonkey = new(i, monkeyInput[0], value);
        else
            newMonkey = new(i, monkeyInput[0], monkeyInput[1]);

        monkeys.Add(newMonkey);
    }

    return monkeys;
}


