using AoCUtils;
using Day21;
using System.Diagnostics;
using System.Numerics;

Console.WriteLine("Day21: ");

string[] input = FileUtil.ReadFileByLine("input.txt");  // part1: 51928383302238  part2: 3305669217840

Stopwatch sw = Stopwatch.StartNew();

//List<Monkey> monkeys = IterationLoop(input, -1);
List<Monkey> monkeys = QueueMethod(input, -1);

Monkey rootPt1 = monkeys.Single(mx => mx.Name == "root");
Console.WriteLine($"[{sw.Elapsed}] Part1: {rootPt1.Value}");


//-----------------------------------------------------------------------------
// root: jhpn + jmsg  <-- from input
// jhpn decreases as humn increases
// binary search humn until value of jhpn == value of jmsg

List<long> trials = new() { 1000000, 10000000, 100000000, 1000000000, 
    10000000000, 100000000000, 1000000000000, 5000000000000 };

Console.WriteLine($"left:jhpn              right:jmsg");
foreach (long trial in trials)
{
    //monkeys = IterationLoop(input, trial);
    monkeys = QueueMethod(input, trial);
    
    Monkey left = monkeys.Single(m => m.Name == "jhpn");
    Monkey right = monkeys.Single(m => m.Name == "jmsg");
    Console.WriteLine($"{left.Value,14}         {right.Value,14}");
}

// binary search
long numLo = 1000000000000;
long numHi = 5000000000000;
long testPoint = 0;

sw = Stopwatch.StartNew();
while (numLo < numHi)
{
    testPoint = (numLo + numHi) / 2;
    //Console.WriteLine($"low:{numLo}  mid:{testPoint}  high:{numHi}");
    Console.WriteLine($"test: {testPoint}");

    //monkeys = IterationLoop(input, testPoint);
    monkeys = QueueMethod(input, testPoint);

    Monkey jhpn = monkeys.Single(m => m.Name == "jhpn");
    Monkey jmsg = monkeys.Single(m => m.Name == "jmsg");

    long result = jmsg.Value - jhpn.Value;
    //Console.WriteLine($"left:{jmsg.Value}  right:{jhpn.Value} diff:{result}");
    
    if (result == 0)
        break;

    if (result < 0)
        numLo = testPoint;
    else
        numHi = testPoint;
}

Console.WriteLine();
Console.WriteLine($"[{sw.Elapsed}] Part2: {testPoint}");
Console.WriteLine();


for (long i = -5; i <= 8; i++)
{
    //monkeys = IterationLoop(input, testPoint + i);
    monkeys = QueueMethod(input, testPoint + i);

    Monkey jhpn = monkeys.Single(m => m.Name == "jhpn");
    Monkey jmsg = monkeys.Single(m => m.Name == "jmsg");
    Monkey root = monkeys.Single(m => m.Name == "root");
    root.op = "-";

    long result = EvaluateExpression(root, jhpn.Value, jmsg.Value);

    Console.WriteLine($"testPoint:{testPoint + i}  result:{root.Value}");
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

List<Monkey> QueueMethod(string[] input, long humnVal)
{
    List<Monkey> resolved = new();
    Queue<Monkey> q = new();
    Monkey tmpMonkey;

    // add monkeys that have a value to resolved list, others go to q
    for (int i = 0; i < input.Length; i++)
    {
        string[] monkeyInput = input[i].Split(": ");

        if (long.TryParse(monkeyInput[1], out long value))
        {
            tmpMonkey = new(i, monkeyInput[0], value);

            // for part2
            if (tmpMonkey.Name == "humn" && humnVal > 0)
                tmpMonkey.Value = humnVal;

            resolved.Add(tmpMonkey);
        }
        else
        {
            tmpMonkey = new(i, monkeyInput[0], monkeyInput[1]);
            q.Enqueue(tmpMonkey);
        }
    }

    // reprocess unresolved monkeys until they're all resolved
    while (q.Count > 0)
    {
        tmpMonkey = q.Dequeue();

        if (resolved.Any(m => m.Name == tmpMonkey.lName) && resolved.Any(m => m.Name == tmpMonkey.rName))
        {
            // both left and right are resolved
            long lValue = resolved.Single(m => m.Name == tmpMonkey.lName).Value;
            long rValue = resolved.Single(m => m.Name == tmpMonkey.rName).Value;

            // resolve and add to resolved list
            EvaluateExpression(tmpMonkey, lValue, rValue);
            resolved.Add(tmpMonkey);
        }
        else
        {
            // put back in q queue
            q.Enqueue(tmpMonkey);
        }
    }

    return resolved;
}

List<Monkey> IterationLoop(string[] input, long humnVal)
{
    List<Monkey> resolved = ReadInput(input);

    // for part2
    if (humnVal > 0)
    {
        Monkey humn = resolved.Single(m => m.Name == "humn");
        humn.Value = humnVal;
    }

    bool moreIterationsNeeded = true;
    
    while (moreIterationsNeeded)
    {
        int resolveCount = 0;

        foreach (Monkey m in resolved)
        {
            // skip if monkey as value or named "humn"
            if (m.Value > 0 || m.Name == "humn")
                continue;

            Monkey lm = resolved.Single(mx => mx.Name == m.lName);
            Monkey rm = resolved.Single(mx => mx.Name == m.rName);

            //Console.Write("checking left and right monkeys: ");
            //Console.WriteLine($"{lm.Name}:{lm.Value}  {rm.Name}:{rm.Value}");
            if (lm.Value > 0 && rm.Value > 0)
            {
                EvaluateExpression(m, lm.Value, rm.Value);
                resolveCount++;
            }
        }

        if (resolveCount == 0)
            moreIterationsNeeded = false;
    }

    return resolved;


    
}

long EvaluateExpression(Monkey m, long lValue, long rValue)
{
    long result = 0;

    if (lValue > 0 && rValue > 0)
    {
        //Console.WriteLine($"-- resolving {m.Name}: {lm.Name}({lm.Value}) {m.op} {rm.Name}({rm.Value})");
        switch (m.op)
        {
            case "+": result = lValue + rValue; break;
            case "-": result = lValue - rValue; break;
            case "*": result = lValue * rValue; break;
            case "/": result = lValue / rValue; break;
            case "=": result = lValue == rValue ? 1 : 0; break;
            default: Console.WriteLine("*** invalid operator"); break;
        }
        m.Value = result;
    }

    return result;
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


