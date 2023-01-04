using AoCUtils;
using System.Text.RegularExpressions;

// Solution converted from hyper-neutrino's python code. Huge learning experience for me.

Console.WriteLine("Day16: Proboscidea Volcanium");

string[] input = FileUtil.ReadFileByLine("input.txt");      // Part1: 2119  Part2: 2615

// parse input and store valve and tunnel information in dictionaries
// keyed by valve name - valves returns flow rate
// keyed by valve name - tunnels returns list of adjacent valves
Dictionary<string, int> valves = new();
Dictionary<string, List<string>> tunnels = new();
foreach (string line in input)
{
    MatchCollection reValves = Regex.Matches(line, @"[A-Z][A-Z]");
    Match reFlow = Regex.Match(line, @"\d+");

    List<string> adjacents = new();
    for (int i = 1; i < reValves.Count; i++)
        adjacents.Add(reValves[i].Value);

    valves.Add(reValves[0].Value, int.Parse(reFlow.Value));
    tunnels.Add(reValves[0].Value, adjacents);
}

// compress graph to only include valves with flow rate > 0
// record distances between these valves and AA
List<string> nonEmpty = new();
Dictionary<string, Dictionary<string, int>> distances = new();
foreach (var v in valves)
{
    if (v.Key != "AA" && valves[v.Key] == 0)
        continue;

    if (v.Key != "AA")
         nonEmpty.Add(v.Key);

    List<string> visited = new() { v.Key };
    Dictionary<string, int> aa = new() { { v.Key, 0 } };
    distances.Add(v.Key, aa);

    Queue<(int d, string p)> q = new();
    q.Enqueue((0, v.Key));

    while (q.Count > 0)
    {
        (int distance, string position) = q.Dequeue();
        
        foreach (var neighbor in tunnels[position])
        {
            if (visited.Contains(neighbor))
                continue;

            visited.Add(neighbor);

            if (valves[neighbor] > 0)
                distances[v.Key].Add(neighbor, distance + 1);

            q.Enqueue((distance + 1, neighbor));
        }
    }

    distances[v.Key].Remove(v.Key);
}

// create a mapping of valves to bits in bit field
Dictionary<string, int> indices = new();
for (int i = 0; i < nonEmpty.Count; i++)
    indices[nonEmpty[i]] = i;

// create a cache to save compute time
Dictionary<(int time, string valve, int mask), int> cache= new();


int answerPt1 = DFS(30, "AA", 0);
Console.WriteLine($"Part1: {answerPt1}");

// set up for part2
cache.Clear();
int splitBits = (1 << nonEmpty.Count) - 1;
int answerPt2 = 0;

for (int i = 0; i < (splitBits + 1) / 2; i++)
    answerPt2 = Math.Max(answerPt2, DFS(26, "AA", i) + DFS(26, "AA", (splitBits ^ i)));

Console.WriteLine($"Part2: {answerPt2}");


//=============================================================================


// Depth First Search
// time = remaining time
// valve = position we're searching from
// vBitField = valve bit field of valves that are already closed
int DFS(int time, string valve, int vBitField)
{
    // if cache has this combination, return the result
    if (cache.ContainsKey((time, valve, vBitField)))
        return cache[(time, valve, vBitField)];

    // search for the path that results in the most remaining time
    int maxVal = 0;
    foreach (var neighbor in distances[valve])
    {
        int bit = 1 << indices[neighbor.Key];
        
        if ((vBitField & bit) != 0)
            continue;

        int remTime = time - distances[valve][neighbor.Key] - 1;
        if (remTime <= 0)
            continue;
        maxVal = Math.Max(maxVal, DFS(remTime, neighbor.Key, vBitField | bit) + valves[neighbor.Key] * remTime);
    }

    // cache this combination and value in case we see it again
    cache[(time, valve, vBitField)] = maxVal;

    return maxVal;
}

