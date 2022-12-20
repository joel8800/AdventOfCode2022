using AoCUtils;
using System.Text.Json;

Console.WriteLine("Day13: Distress Signal");

string[] inputPairs = FileUtil.ReadFileByBlock("input.txt");    // pt1: 5808  pt2: 22713
string[] inputLines = FileUtil.ReadFileByLine("input.txt");

List<List<int>> left = new();
List<List<int>> right = new();

int total = 0;
for (int i = 0; i < inputPairs.Length; i++)
{
    string block = inputPairs[i];
    string[] pair = block.Split(Environment.NewLine);

    JsonElement leftJSON = JsonDocument.Parse(pair[0]).RootElement;
    JsonElement rightJSON = JsonDocument.Parse(pair[1]).RootElement;

    int result = comparePackets(leftJSON, rightJSON);
    if (result < 0)
        total += (i + 1);
}

Console.WriteLine($"Part1: {total}");

List<JsonElement> packets = new();
foreach (string line in inputLines)
{
    if (line == "")
        continue;

    JsonElement packet = JsonDocument.Parse(line).RootElement;
    packets.Add(packet);
}

//packets.Add(JsonDocument.Parse("[[2]]").RootElement);
//packets.Add(JsonDocument.Parse("[[6]]").RootElement);
JsonElement twoPacket = JsonDocument.Parse("[[2]]").RootElement;
JsonElement sixPacket = JsonDocument.Parse("[[6]]").RootElement;

int index2 = 1;
int index6 = 2;
foreach (var p in packets)
{
    int result2 = comparePackets(twoPacket, p);
    //Console.WriteLine($"two vs p: {result2}");
    if (result2 > 0)
        index2++;

    int result6 = comparePackets(sixPacket, p);
    //Console.WriteLine($"six vs p: {result6}");
    if (result6 > 0) 
        index6++;
}
//Console.WriteLine($"index2:{index2}  index6:{index6}");
Console.WriteLine($"Part2: {index2 * index6}");


int comparePackets(JsonElement left, JsonElement right)
{
    if (left.ValueKind == JsonValueKind.Number && right.ValueKind == JsonValueKind.Number)
    {
        return left.GetInt32() - right.GetInt32();
    }
    else if (left.ValueKind == JsonValueKind.Number)
    {
        return comparePackets(JsonDocument.Parse($"[{left.GetInt32()}]").RootElement, right);
    }
    else if (right.ValueKind == JsonValueKind.Number)
    {
        return comparePackets(left, JsonDocument.Parse($"[{right.GetInt32()}]").RootElement);
    }
    else
    {
        foreach (var (nextLeft, nextRight) in Enumerable.Zip(left.EnumerateArray(), right.EnumerateArray()))
        {
            var current = comparePackets(nextLeft, nextRight);
            if (current == 0)
                continue;
            else
                return current;
        }

        return left.GetArrayLength() - right.GetArrayLength();
    }
}