using AoCUtils;
using Day20;

Console.WriteLine("Day20: Grove Positioning System");

string[] input = FileUtil.ReadFileByLine("input.txt");  // part1: 4426  part2: 8119137886612

List<long> origOrder = new();
DblLinkedList encFilePt1 = new();

for (int i = 0; i < input.Length; i++)
{
    long value = Convert.ToInt64(input[i]);
    origOrder.Add(value);
    Node newNode = new(i, value);

    if (i == 0)
        encFilePt1.AddFirst(newNode);
    else
        encFilePt1.AddAfter(encFilePt1.Tail, newNode);
}

//PrintList(encFilePt1);
int m = encFilePt1.Count - 1;  // circular so need 1 less than number of nodes to return to original place

for (int j = 0; j < encFilePt1.Count; j++)
{
    Node origNode, nextNode;
    origNode = encFilePt1.FindId(j);
    nextNode = origNode;

    long num = origNode.Value;

    if (num == 0)
        continue;

    long numValue = num % m;

    if (num > 0)
    {
        for (int k = 0; k < numValue; k++)
            nextNode = nextNode.Next;

        encFilePt1.Remove(origNode);
        encFilePt1.AddAfter(nextNode, origNode);
    }
    else
    {
        for (int k = 0; k < -numValue; k++)
            nextNode = nextNode.Prev;

        encFilePt1.Remove(origNode);
        encFilePt1.AddBefore(nextNode, origNode);
    }
}

Node zero = encFilePt1.FindZero();
Node mover = zero;
long index1k = 0, index2k = 0, index3k = 0;
for (int i = 1; i <= 5000; i++)
{
    mover = mover.Next;

    if (i == 1000)
        index1k = mover.Value;
    if (i == 2000)
        index2k = mover.Value;
    if (i == 3000)
        index3k = mover.Value;
}

//Console.WriteLine($"1k:[{index1k}]  2k:[{index2k}]  3k:[{index3k}]");
long answerPt1 = index1k + index2k + index3k;
Console.WriteLine($"Part1: {answerPt1}");

//-----------------------------------------------------------------------------

DblLinkedList encFilePt2 = new();

for (int i = 0; i < input.Length; i++)
{
    long value = Convert.ToInt64(input[i]) * 811589153L;    // multiply by decryption key
    origOrder.Add(value);
    Node newNode = new(i, value);

    if (i == 0)
        encFilePt2.AddFirst(newNode);
    else
        encFilePt2.AddAfter(encFilePt2.Tail, newNode);
}

//PrintList(encFilePt2);
//int m = encFilePt2.Count - 1;  // circular so need 1 less than number of nodes to return to original place

for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < encFilePt2.Count; j++)
    {
        Node origNode, nextNode;
        origNode = encFilePt2.FindId(j);
        nextNode = origNode;

        long num = origNode.Value;

        if (num == 0)
            continue;

        long numValue = num % m;

        if (num > 0)
        {
            for (int k = 0; k < numValue; k++)
                nextNode = nextNode.Next;

            encFilePt2.Remove(origNode);
            encFilePt2.AddAfter(nextNode, origNode);
        }
        else
        {
            for (int k = 0; k < -numValue; k++)
                nextNode = nextNode.Prev;

            encFilePt2.Remove(origNode);
            encFilePt2.AddBefore(nextNode, origNode);
        }
    }
    //Console.WriteLine();
    //PrintList(encFilePt2);
}

zero = encFilePt2.FindZero();
mover = zero;
index1k = 0; index2k = 0; index3k = 0;
for (int i = 1; i <= 3000; i++)
{
    mover = mover.Next;

    if (i == 1000)
        index1k = mover.Value;
    if (i == 2000)
        index2k = mover.Value;
    if (i == 3000)
        index3k = mover.Value;
}

//Console.WriteLine($"1k:[{index1k}]  2k:[{index2k}]  3k:[{index3k}]");
long answerPt2 = index1k + index2k + index3k;
Console.WriteLine($"Part2: {answerPt2}");

//=============================================================================

void PrintList(DblLinkedList list)
{
    int count = list.Count;
    Node rover = list.Head;

    for (int i = 0; i < count; i++)
    {
        Console.WriteLine(rover.Value);
        rover = rover.Next;
    }
    
}