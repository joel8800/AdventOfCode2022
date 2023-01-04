using AoCUtils;

Console.WriteLine("Day25: Full of Hot Air");

string[] input = FileUtil.ReadFileByLine("input.txt");  // Part1: 2--2-0=--0--100-=210

long total = 0;
foreach (string sNum in input)
{
    long decNum = Snafu2Decimal(sNum);

    //// test
    //string snafu = Decimal2Snafu(decNum);
    //Console.WriteLine($"decNum: {decNum}  snafuNum: {snafu}");

    total += decNum;
}

string answerPt1 = Decimal2Snafu(total);

Console.WriteLine($"Part1: {answerPt1}");

//=============================================================================

long Snafu2Decimal(string snafuNum)
{
    Dictionary<char, int> s2d = new()
    { { '2', 2 }, { '1', 1 }, { '0', 0 }, { '-', -1 }, { '=', -2 } };

    char[] revSn = snafuNum.Reverse().ToArray();

    long total = 0;
    for (int i = 0; i < revSn.Length; i++)
    {
        total += s2d[revSn[i]] * Convert.ToInt64(Math.Pow(5, i));
    }

    return total;
}

string Decimal2Snafu(long decNum)
{
    Dictionary<int, char> d2s = new()
    { { 2, '2' }, { 1, '1' }, { 0, '0' }, { -1, '-' }, { -2, '=' } };

    List<char> snafuList = new();

    while (decNum > 0)
    {
        int digit = Convert.ToInt32(((decNum + 2) % 5) - 2);
        snafuList.Add(d2s[digit]);
        
        decNum -= digit;
        decNum /= 5;
    }

    snafuList.Reverse();

    string snafu = string.Empty;
    foreach (char c in snafuList)
        snafu += c;

    return snafu;
}