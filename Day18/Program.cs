using AoCUtils;
using Day18;
using System.Diagnostics;

Console.WriteLine("Day18: Boiling Boulders");


string[] input = FileUtil.ReadFileByLine("input.txt");  // pt1: 64, 4242  pt2: 58, 2428 

//var data =
//    (from line in File.ReadAllLines("input.txt")
//     where !string.IsNullOrWhiteSpace(line)
//     let cube = line.Split(",").Select(int.Parse).ToArray()
//     select (x: cube[0], y: cube[1], z: cube[2])).ToHashSet();

//var minX = data.Select(p => p.x).Min();
//var maxX = data.Select(p => p.x).Max();
//var minY = data.Select(p => p.y).Min();
//var maxY = data.Select(p => p.y).Max();
//var minZ = data.Select(p => p.z).Min();
//var maxZ = data.Select(p => p.z).Max();

HashSet<Cube> cubeList = new();

foreach (string line in input)
{
    string[] xyz = line.Trim().Split(',');
    int x = int.Parse(xyz[0]);
    int y = int.Parse(xyz[1]);
    int z = int.Parse(xyz[2]);
    //Console.WriteLine($"x:{x} y:{y} z:{z}");
    cubeList.Add(new(x, y, z));
}
//int minX = cubeList.Select(c => c.X).Min();
//int minY = cubeList.Select(c => c.Y).Min();
//int minZ = cubeList.Select(c => c.Z).Min();
//int maxX = cubeList.Select(c => c.X).Max();
//int maxY = cubeList.Select(c => c.Y).Max();
//int maxZ = cubeList.Select(c => c.Z).Max();
//Console.WriteLine($"{cubeList.Count}");
//Console.WriteLine($"min[{minX},{minY},{minZ}]  max[{maxX},{maxY},{maxZ}]");

Stopwatch sw = Stopwatch.StartNew();
int resultPt1 = 0;
foreach (Cube cube in cubeList)
{
    int x = cube.X; int y = cube.Y; int z = cube.Z;
    //Console.Write($"[{x},{y},{z}] ");

    Cube cmx = new(x + 1, y, z);
    Cube cpx = new(x - 1, y, z);
    Cube cmy = new(x, y + 1, z);
    Cube cpy = new(x, y - 1, z);
    Cube cmz = new(x, y, z + 1);
    Cube cpz = new(x, y, z - 1);

    if (cubeList.Contains(cmx) == false)
        resultPt1++;
    if (cubeList.Contains(cpx) == false)
        resultPt1++;
    if (cubeList.Contains(cmy) == false)
        resultPt1++;
    if (cubeList.Contains(cpy) == false)
        resultPt1++;
    if (cubeList.Contains(cmz) == false)
        resultPt1++;
    if (cubeList.Contains(cpz) == false)
        resultPt1++;
    //Console.WriteLine($"{resultPt1}");
}
Console.WriteLine($"[{sw.Elapsed}] Part1: {resultPt1}");

HashSet<Cube> inside = new();
HashSet<Cube> outside = new();
//HashSet<Cube> visited = new();
int called = 0;

sw = Stopwatch.StartNew();
int resultPt2 = 0;
foreach (Cube cube in cubeList)
{
    int x = cube.X; int y = cube.Y; int z = cube.Z;

    //Console.WriteLine($"===== {cube} =============");
    Cube cpx = new(x + 1, y, z);
    Cube cmx = new(x - 1, y, z);
    Cube cpy = new(x, y + 1, z);
    Cube cmy = new(x, y - 1, z);
    Cube cpz = new(x, y, z + 1);
    Cube cmz = new(x, y, z - 1);

    //Console.WriteLine($"Calling ExternalFacing with {cpx}");
    if (ExternalFacing(cpx)) //, cubeList, inside, outside))
    {
        //Console.WriteLine($"ExternalFacing {cpx}");
        resultPt2++;
    }
    //Console.WriteLine($"Calling ExternalFacing with {cmx}");
    if (ExternalFacing(cmx)) //, cubeList, inside, outside))
    {
       //Console.WriteLine($"ExternalFacing {cmx}");
        resultPt2++;
    }
    //Console.WriteLine($"Calling ExternalFacing with {cpy}");
    if (ExternalFacing(cpy)) //, cubeList, inside, outside))
    {
        //Console.WriteLine($"ExternalFacing {cpy}");
        resultPt2++;
    }
    //Console.WriteLine($"Calling ExternalFacing with {cmy}");
    if (ExternalFacing(cmy)) //, cubeList, inside, outside))
    {
        //Console.WriteLine($"ExternalFacing {cmy}");
        resultPt2++;
    }
    //Console.WriteLine($"Calling ExternalFacing with {cpz}");
    if (ExternalFacing(cpz)) //, cubeList, inside, outside))
    {
        //Console.WriteLine($"ExternalFacing {cpz}");
        resultPt2++;
    }
    //Console.WriteLine($"Calling ExternalFacing with {cmz}");
    if (ExternalFacing(cmz)) //, cubeList, inside, outside))
    {
        //Console.WriteLine($"ExternalFacing {cmz}");
        resultPt2++;
    }

    //Console.WriteLine($"inside:{inside.Count}  outside:{outside.Count}");
    //Console.WriteLine();
}
//Console.WriteLine($"ExternalFacing was called {called} times");
Console.WriteLine($"[{sw.Elapsed}] Part2: {resultPt2}");


bool ExternalFacing(Cube cube)
{
    called++;
    //Console.WriteLine($"----- {cube} -----");

    if (outside.Contains(cube))
    {
        //Console.WriteLine($"{cube} found in outside, returning True");
        return true;
    }
    if (inside.Contains(cube))
    {
        //Console.WriteLine($"{cube} found in inside, returning False");
        return false;
    }

    HashSet<Cube> visited = new();

    Queue<Cube> q = new();
    q.Enqueue(cube);

    while (q.Count > 0)
    {
        Cube qCube = q.Dequeue();
        //Console.WriteLine($"{qCube}: checking from queue");

        if (cubeList.Contains(qCube))
        {
            //Console.WriteLine($"{qCube} found in cubeList, continuing");
            continue;
        }

        if (visited.Contains(qCube))
        {
            //Console.WriteLine($"{qCube} found in visited, continuing");
            continue;
        }

        visited.Add(qCube);
        //Console.WriteLine($"{qCube} added to visited: {visited.Count}");
        if (visited.Count > 5000)
        {
            foreach (Cube c in visited)
            {
                outside.Add(c);
                //Console.WriteLine($"({c.X}, {c.Y}, {c.Z}) in visited, added to outside");
            }
            return true;
        }

        int x = qCube.X; int y = qCube.Y; int z = qCube.Z;
        Cube cpx = new(x + 1, y, z);
        Cube cmx = new(x - 1, y, z);
        Cube cpy = new(x, y + 1, z);
        Cube cmy = new(x, y - 1, z);
        Cube cpz = new(x, y, z + 1);
        Cube cmz = new(x, y, z - 1);

        q.Enqueue(cpx); //Console.WriteLine($"enqueue {cpx}");
        q.Enqueue(cmx); //Console.WriteLine($"enqueue {cmx}");
        q.Enqueue(cpy); //Console.WriteLine($"enqueue {cpy}");
        q.Enqueue(cmy); //Console.WriteLine($"enqueue {cmy}");
        q.Enqueue(cpz); //Console.WriteLine($"enqueue {cpz}");
        q.Enqueue(cmz); //Console.WriteLine($"enqueue {cmz}");
    }

    foreach (Cube c in visited)
    {
        inside.Add(c);
        //Console.WriteLine($"({c.X}, {c.Y}, {c.Z}) in visited, added to inside");
    }

    return false;
}
