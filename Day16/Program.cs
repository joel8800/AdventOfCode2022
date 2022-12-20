using AoCUtils;
using Day16;
using System.Text;

Console.WriteLine("Day16: Proboscidea Volcanium");

string[] input = FileUtil.ReadFileByLine("inputSamp.txt");

ValveSystem tunnels = new();

foreach (string line in input)
{
    string[] sides = line.Split(';', StringSplitOptions.TrimEntries);
    string[] valveInfo = sides[0].Split(' ');
    string[] rateInfo = valveInfo[4].Split("=");
    string[] adjacents = sides[1].Split("valve");
    
    ValveRoom v = new(valveInfo[1], rateInfo[1]);
    v.AddAdjacents(adjacents[1]);

    tunnels.Valves.Add(v);
    Console.WriteLine(v);
}

tunnels.ConnectTunnels();

int released = 0;

int step = 0;
ValveRoom us = tunnels.Valves[0];

//while (step < 30)
//{
//    released += GetReleasedPressure(tunnels);
    
//    // if we're here check if we can open valve
//    if (us.Rate > 0 && us.OnOff == false)
//    {
//        us.OnOff = true;
//        step++;
//        continue;
//    }
//    else
//    {
//        // look for best path 
//        List<int> score = new();
//        for (int i = 0; i < us.AdjNodes.Count; i++)
//        {
//            score.Add(GetPathCost(us.AdjNodes[i], 1));
//        }
//        for (int i = 0; i < us.AdjNodes.Count; i++)
//        {
//            Console.WriteLine($"score[{i}]: {score[i]}");
//        }
//    }

//    step++;
//}





Console.WriteLine($"Part1: {released}");








//=============================================================================

int GetPathCost(ValveRoom adj, int level)
{
    if (adj.Rate > 0 && adj.OnOff == false)
    {
        return adj.Rate * level;
    }

    List<int> score = new();
    for (int i = 0; i < adj.AdjNodes.Count; i++)
    {
        score.Add(GetPathCost(adj.AdjNodes[i], level + 1));
    }
    return score.Max();
}



int GetReleasedPressure(ValveSystem vs)
{
    int released = 0;
    StringBuilder sb = new();
    sb.Append("opened: ");

    foreach (ValveRoom v in vs.Valves)
    {
        if (v.OnOff == true)
            released += v.Rate;
        sb.Append($"{v.ID} ");
    }
    sb.Append($": {released}");
    Console.WriteLine(sb.ToString());

    return released;
}


