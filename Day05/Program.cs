using AoCUtils;
using System.Text;

Console.WriteLine("Day05: Supply Stacks");

const int NUMSTACKS = 9;

string[] input = FileUtil.ReadFileByBlock("input.txt");

string[] stackInputs = input[0].Split(Environment.NewLine);
string[] instructions = input[1].Split(Environment.NewLine);

List<Stack<char>> stacksPt1 = new List<Stack<char>>();
List<Stack<char>> stacksPt2 = new List<Stack<char>>();

for (int i = 0; i < NUMSTACKS; i++)
{
    stacksPt1.Add(new Stack<char>());
    stacksPt2.Add(new Stack<char>());
}

// load stacks with crates from stackInputs
// load from bottom up, don't push stack numbers
for (int i = stackInputs.Length - 2; i >= 0; i--)
{
    string line = stackInputs[i];

    for (int j = 0; j < NUMSTACKS; j++)
    {
        if (line.Length > (j * 4 + 2))
            if (line[j * 4 + 1] != ' ')
            {
                stacksPt1[j].Push(line[j * 4 + 1]);
                stacksPt2[j].Push(line[j * 4 + 1]);
            }
    }
}

// part 1
// parse instruction and move crates one at a time
foreach (string instruction in instructions)
{
    string[] instPart = instruction.Split(' ');

    int numToMove = Convert.ToInt32(instPart[1]);
    int fromStack = Convert.ToInt32(instPart[3]) - 1;
    int destStack = Convert.ToInt32(instPart[5]) - 1;
    
    for (int i = 0; i < numToMove; i++)
        stacksPt1[destStack].Push(stacksPt1[fromStack].Pop());
}

StringBuilder sb = new();
for (int i = 0; i < NUMSTACKS; i++)
{
    stacksPt1[i].TryPeek(out char c);
    sb.Append(c);
}
Console.WriteLine($"Part1: {sb}");

// part 2
// parse instruction and move crates in groups using another stack to maintain order
foreach (string instruction in instructions)
{
    string[] instPart = instruction.Split(' ');
    Stack<char> moverStack = new();

    int numToMove = Convert.ToInt32(instPart[1]);
    int fromStack = Convert.ToInt32(instPart[3]) - 1;
    int destStack = Convert.ToInt32(instPart[5]) - 1;
    
    // pop crates to move, this reverses order
    for (int i = 0; i < numToMove; i++)
        moverStack.Push(stacksPt2[fromStack].Pop());

    // push to new stack, this restores order
    for (int i = 0; i < numToMove; i++)
        stacksPt2[destStack].Push(moverStack.Pop());
}

StringBuilder sb2 = new();
for (int i = 0; i < NUMSTACKS; i++)
{
    stacksPt2[i].TryPeek(out char c);
    sb2.Append(c);
}
Console.WriteLine($"Part2: {sb2}");

