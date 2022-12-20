using AoCUtils;
using Day11;

Console.WriteLine("Day11: Monkey in the Middle");

long part1 = MonkeyBusiness(true);
long part2 = MonkeyBusiness(false);

Console.WriteLine($"Part1: {part1}");       // pt1:10605       pt2:117640
Console.WriteLine($"Part2: {part2}");       // pt1:2713310158  pt2:30616425600

//=============================================================================

long MonkeyBusiness(bool isPart1)
{
    string[] inputs = FileUtil.ReadFileByBlock("input.txt");

    List<Monkey> monkeys = new();
    long lcd = 1;
    foreach (string input in inputs)
    {
        Monkey newMonkey = new(input);

        newMonkey.IsPart1 = isPart1;
        lcd *= newMonkey.Divisor;

        monkeys.Add(newMonkey);
    }

    List<int> markers = new();
    //List<int> markers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20, 
        //1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000 };

    for (int i = 1; i <= 10000; i++)
    {
        foreach (Monkey monkey in monkeys)
        {
            monkey.LCD = lcd;
            monkey.InspectItems();

            // send thrown items to other Monkeys
            while (monkey.OutItems.Count > 0)
            {
                int monkeyIndex = monkey.OutMonkeys.Dequeue();
                long tossedItem = monkey.OutItems.Dequeue();
                monkeys[monkeyIndex].AddItemToQueue(tossedItem);
            }
        }

        if (markers.Exists(x => x == i))
            PrintInspections(monkeys, i);

        if (isPart1 && i >= 20)
            break;
    }

    List<long> inspections = new();
    foreach (Monkey monkey in monkeys)
    {
        inspections.Add(monkey.Inspections);
        //Console.WriteLine($"Monkey {monkey.ID} inspected items {monkey.Inspections} times.");
    }

    long answer = inspections.Max();
    inspections.Remove(answer);
    answer *= inspections.Max();

    return answer;
}

#pragma warning disable 8321      // this is a stupid warning that needs to be suppressed
void PrintInspections(List<Monkey> monkeys, int round)
{
    Console.WriteLine($"== After round {round} ==");
    foreach (Monkey monkey in monkeys)
    {
        monkey.PrintMonkey();
    }
    foreach (Monkey monkey in monkeys)
    {
        Console.WriteLine($"Monkey {monkey.ID} inspected items {monkey.Inspections} times.");
    }
    Console.WriteLine();
}
#pragma warning restore 8321