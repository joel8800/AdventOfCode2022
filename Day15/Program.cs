using AoCUtils;
using Day15;
using System.Text.RegularExpressions;

Console.WriteLine("Day15: Beacon Exclusion Zone");

string[] input = FileUtil.ReadFileByLine("input.txt");
int min = 0;
int max = 4000000;
int rowOfInterest = 2000000;

List<Sensor> sensors= new();
HashSet<Beacon> beacons= new();

Regex re = new(@"-*[0-9]+");


foreach (string line in input)
{
    MatchCollection mc = Regex.Matches(line, @"-*[0-9]+");
    int xs = Convert.ToInt32(mc[0].Value);
    int ys = Convert.ToInt32(mc[1].Value);
    int xb = Convert.ToInt32(mc[2].Value);
    int yb = Convert.ToInt32(mc[3].Value);
    Sensor sens = new(xs, ys, xb, yb);
    sensors.Add(sens);
    Beacon beac = new(xb, yb);
    beacons.Add(beac);
    //Console.WriteLine($"S[{sens.X},{sens.Y}] B[{sens.bX},{sens.bY}] dist:{sens.BeaconDist}");
}


// check each sensor to see if it overlaps with row of interest
// save the interval of x locations that intersect with row
List<Interval> intervalList = new();
foreach (Sensor s in sensors)
{
    int sensorToRow = Math.Abs(s.Y - rowOfInterest);
    if (sensorToRow <= s.BeaconDist)
    {
        int xOffset = s.BeaconDist - sensorToRow;
        int xLo = s.X - xOffset;
        int xHi = s.X + xOffset;

        Interval i = new(xLo, xHi);
        intervalList.Add(i);

        //Console.Write($"[{s.X},{s.Y}] beacon dist:{s.BeaconDist}, to row {rowOfInterest}:");
        //Console.WriteLine($" {sensorToRow}, X interval: {i.Lo} to {i.Hi}");
    }
}
Console.WriteLine();

// sort the intervals to prepare for merging
intervalList.Sort();

// merge intervals in the row
int index = 0;
bool moreMerges = true;
while (moreMerges)
{
    if (intervalList.Count > 1)
    {
        if (intervalList.Count == 2)
        {
            List<Interval> m = MergeIntervals(intervalList[0], intervalList[1]);

            if (m.Count == 1)
            {
                intervalList.RemoveAt(index + 1);
                intervalList.RemoveAt(index);
                intervalList.Insert(0, m[0]);
            }
            moreMerges = false;
        }
        else
        {
            List<Interval> m = MergeIntervals(intervalList[index], intervalList[index + 1]);

            if (m.Count == 1 && index <= intervalList.Count - 2)
            {
                intervalList.RemoveAt(index + 1);
                intervalList.RemoveAt(index);
                intervalList.Insert(index, m[0]);
                continue;
            }
            else
            {
                if (intervalList.Count <= 2)
                    moreMerges = false;
                else
                    index++;
            }
        }
    }
}

// remove any beacons on row of interest
int beaconsToRemove = 0;
foreach (Beacon b in beacons)
    if (b.Y == rowOfInterest)
        beaconsToRemove++;

Console.WriteLine($"Part1: {(intervalList[0].Hi + 1) - intervalList[0].Lo - beaconsToRemove}"); // part1: 4951427
//-----------------------------------------------------------------------------

List<Interval> valid = new();
for (rowOfInterest = min; rowOfInterest < max; rowOfInterest++)
{
    // check each sensor to see if it overlaps with row of interest
    // save the interval of x locations that intersect with row
    intervalList.Clear();
    foreach (Sensor s in sensors)
    {
        int sensorToRow = Math.Abs(s.Y - rowOfInterest);    // ManhattanDist(s, s.X, rowOfInterest);
        if (sensorToRow <= s.BeaconDist)
        {
            int xOffset = s.BeaconDist - sensorToRow;
            int xLo = Math.Max(s.X - xOffset, min);
            int xHi = Math.Min(s.X + xOffset, max);

            Interval i = new(xLo, xHi);
            intervalList.Add(i);

            //Console.WriteLine($"[{s.X},{s.Y}] beacon dist:{s.BeaconDist}, to row {rowOfInterest}: {sensorToRow}, X interval: {i.Lo} to {i.Hi}");
        }
    }

    // sort the intervals to prepare for merging
    intervalList.Sort();

    // merge intervals in the row
    index = 0;
    moreMerges = true;
    while (moreMerges)
    {
        if (intervalList.Count > 1)
        {
            if (intervalList.Count == 2)
            {
                List<Interval> m = MergeIntervals(intervalList[0], intervalList[1]);

                if (m.Count == 1)
                {
                    intervalList.RemoveAt(index + 1);
                    intervalList.RemoveAt(index);
                    intervalList.Insert(0, m[0]);
                }
                moreMerges = false;
            }
            else
            {
                List<Interval> m = MergeIntervals(intervalList[index], intervalList[index + 1]);

                if (m.Count == 1 && index <= intervalList.Count - 2)
                {
                    intervalList.RemoveAt(index + 1);
                    intervalList.RemoveAt(index);
                    intervalList.Insert(index, m[0]);
                    continue;
                }
                else
                {
                    if (intervalList.Count <= 2)
                        moreMerges = false;
                    else
                        index++;
                }
            }
        }
    }

    if (intervalList.Count == 2)
    {
        valid.Add(new(intervalList[0].Hi + 1, rowOfInterest));
    }

}

//Console.WriteLine(valid[0]);
// x = 3257428, y = 2573243
long x = Convert.ToInt64(valid[0].Lo);
long y = Convert.ToInt64(valid[0].Hi);
Console.WriteLine($"Part2: {(x * 4000000) + y}");       // part2: 13029714573243


//=============================================================================


List<Interval> MergeIntervals(Interval intv1, Interval intv2)
{
    List<Interval> merged = new();

    // check if intv2 is fully contained in intv1
    if (intv1.Hi >= intv2.Lo && intv1.Hi >= intv2.Hi)
    {
        merged.Add(intv1);
        return merged;
    }

    if (intv1.Hi >= intv2.Lo)
    {
        merged.Add(new(intv1.Lo, intv2.Hi));
    }
    else
    {
        merged.Add(intv1);
        merged.Add(intv2);
    }

    return merged;
}
