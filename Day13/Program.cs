using AoCUtils;
using Microsoft.VisualBasic;
using System.Net;
using System.Text.RegularExpressions;

Console.WriteLine("Day13:");

string[] input = FileUtil.ReadFileByBlock("inputSamp.txt");

Regex re = new(@"[\d+\w+]");
List<List<int>> left = new();
List<List<int>> right = new();
char[] delim = { '[', ']' };

int total = 0;
for (int i = 0; i < input.Length; i++)
{
    string block = input[i];
    string[] pair = block.Split(Environment.NewLine);

    left = MakeLists(pair[0]);
    right = MakeLists(pair[1]);

    int result = CompareLeftToRight(left, right);
    total += (i + 1) * result * -1;

    Console.WriteLine($"[{i + 1}] = {result}, total = {total}");
}

Console.WriteLine($"Part1: {total}");


List<List<int>> MakeLists(string input)
{
    List<List<int>> myList = new();

    string[] splits = input.Split(delim);

    foreach (string group in splits)
    {
        if (group == ",")
            continue;

        List<int> tmp = new();

        if (group.Length > 0)
        {
            // add items to tmp
            string[] items = group.Split(',', StringSplitOptions.RemoveEmptyEntries);
            tmp = items.Select(x => Convert.ToInt32(x)).ToList();
        }

        myList.Add(tmp);
    }

    return myList;
}


int CompareLeftToRight(List<List<int>> left, List<List<int>> right)
{
    int i = 0;
    while (left.Count > 1 && right.Count > i)
    {
        int compare = CompareList(left[i], right[i]);

        if (compare == -1)
            return -1;

        if (compare == 1)
            return 1;
        
    }
    if (left.Count == i && right.Count > i)
        return -1;
    else if (right.Count == i && left.Count > i)
        return 1;
    else
        return 0;
}


int CompareInt(int left, int right)
{
    if (left < right)
        return -1;
    else if (left == right)
        return 0;
    else if (left > right)
        return 1;
    return 99;
}

int CompareList(List<int> left, List<int> right)
{
    if (left.Count == 0 && right.Count == 0)        // empty lists match
        return -1;

    int i = 0;
    while (left.Count > i && right.Count > i)
    {
        int compare = CompareInt(left[i], right[i]);
        if (compare == -1)
            return -1;
        if (compare == 1)
            return 1;
        i += 1;
    }
    if (left.Count == i && right.Count > i)
        return -1;
    else if (right.Count == i && left.Count > i)
        return 1;
    else
        return 0;
}












int CompareListToInt(List<int> left, int right)
{
    List<int> rightList = new() { right };

    return CompareList(left, rightList);
}

int CompareIntToList(int left, List<int> right)
{
    List<int> leftList = new() { left };

    return CompareList(leftList, right);
}

string RemoveBrackets(string text)
{
    return text.Substring(1, text.Length - 2);
}