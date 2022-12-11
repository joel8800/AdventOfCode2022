using AoCUtils;
using Day07;
using System.IO.IsolatedStorage;
using System.Reflection.Metadata.Ecma335;

// part1: 1543140
// part2: 1117448

Console.WriteLine("Day07: No Space Left On Device");

string[] instructions = FileUtil.ReadFileByDelimiter("input.txt", "$ ");

string currentPath = "/";
List<string> pathList = new() { currentPath };
DirClass root = new(currentPath);
DirClass currentDir = root;

int numInst = instructions.Length;

// build out directory structure while reading instructions
foreach (string instruction in instructions)
{
    // handle files and sub directories
    if (instruction.StartsWith("ls"))
    {
        List<string> results = instruction.Split(Environment.NewLine, 
                                    StringSplitOptions.RemoveEmptyEntries).ToList();

        results.RemoveAt(0);    // remove the "ls"

        foreach (string item in results)
        {
            if (item.StartsWith("dir"))
            {
                // add new subdir
                string dir = item.Substring(4).Trim();
                DirClass newDir = new(dir);
                newDir.FullPath = GetUpdatedPath(pathList, dir);
                currentDir.SubDirs.Add(newDir);
            }
            else
            {
                // add new file
                string[] fileItems = item.Split(' ');
                int fileSize = Convert.ToInt32(fileItems[0]);
                string fileName = fileItems[1];
                FileClass newFile = new(fileName, fileSize);
                currentDir.Files.Add(newFile);
                currentDir.SpaceUsed += fileSize;
            }
        }
        continue;
    }

    // handle directory navigation
    if (instruction.StartsWith("cd"))
    {
        string dir = instruction.Substring(3).Trim();
        
        // go back to root
        if (dir == "/")
        {
            currentDir = root;
            currentPath = "/";
            continue;
        }

        // move up one level
        if (dir == "..")
        {
            currentDir = MoveUp(pathList);
            continue;
        }

        // move down into subdir
        DirClass nextDir = currentDir.SubDirs.Where(s => s.DirName == dir).First();
        pathList.Add(nextDir.DirName);
        currentDir = nextDir;
    }
}

List<string> dirNames = new();
List<int> dirSizes = new();

int spaceUsed = GetDirectorySizes(root);

List<int> lessThan100k = dirSizes.Where(i => i <= 100000).ToList();
int totalPt1 = lessThan100k.Sum();

Console.WriteLine($"Part1: {totalPt1}");

//-----------------------------------------------------------------------------

int freeSpaceNeeded = 30000000 - (70000000 - spaceUsed);
//Console.WriteLine($"free space needed: {freeSpaceNeeded}");

List<int> dirCandidates = dirSizes.Where(i => i >= freeSpaceNeeded).ToList();
int minSizePt2 = dirCandidates.Min();

Console.WriteLine($"Part2: {minSizePt2}");


// ============================================================================

string GetUpdatedPath(List<string> currPath, string dir)
{
    int stackSize = currPath.Count;
    string pathString = string.Empty;

    for (int i = 0; i < stackSize; i++)
    {
        pathString += currPath[i];
        if (pathString.EndsWith('/') == false)
            pathString += "/";
    }
    pathString += dir + "/";

    return pathString;
}

DirClass MoveUp(List<string> currPath)
{
    int pathLevels = currPath.Count;
    DirClass ptr = root;

    for (int i = 1; i < currPath.Count - 1; i++)
    {
        ptr = ptr.SubDirs.Where(s => s.DirName == currPath[i]).First();
    }

    currPath.RemoveAt(currPath.Count - 1);
    return ptr;
}

int GetDirectorySizes(DirClass thisDir)
{
    int subDirSize = 0;

    foreach (DirClass d in thisDir.SubDirs)
    {
        subDirSize += GetDirectorySizes(d);
    }
    thisDir.SpaceUsed += subDirSize;

    // add directory entry to list
    dirSizes.Add(thisDir.SpaceUsed);
    
    return thisDir.SpaceUsed;
}
