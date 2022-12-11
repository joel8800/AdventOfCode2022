using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    public class Monkey
    {
        public int ID { get; set; }
        public string Operation { get; set; }
        public long Divisor { get; set; }
        public int Inspections { get; set; }
        public int TargetTrue { get; set; }
        public int TargetFalse { get; set; }
        public bool IsPart1 { get; set; }
        public long LCD { get ; set; }
        public Queue<long> Items { get; set; }
        public Queue<int> OutMonkeys { get; set; }
        public Queue<long> OutItems { get; set; }

        public Monkey(string input)
        {
            // split Monkey block into lines
            string[] lines = input.Split(Environment.NewLine);

            // get ID
            string[] tokens = lines[0].Replace(':', ' ').Split(' ');
            ID = Convert.ToInt32(tokens[1]);

            // init queue for items he has and fill it
            Items = new();
            tokens = lines[1].Split(":");
            string itemString = tokens[1];
            tokens = itemString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                Items.Enqueue(Convert.ToInt64(token.Trim()));
            }

            // get the operation
            tokens = lines[2].Split(":");
            Operation = tokens[1].Trim();

            // get the divisor to check against
            tokens = lines[3].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Divisor = Convert.ToInt64(tokens[3]);

            // get the true and false target Monkeys
            tokens = lines[4].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            TargetTrue = Convert.ToInt32(tokens[5]);
            tokens = lines[5].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            TargetFalse = Convert.ToInt32(tokens[5]);

            // init queue for outgoing items
            OutMonkeys = new();
            OutItems = new();

            Inspections = 0;
        }

        public void PrintMonkey()
        {
            Console.Write($"Monkey{ID}: ");
            //Console.WriteLine($"operation: {Operation}");
            //Console.WriteLine($"divisor  : {Divisor}");
            //Console.WriteLine($"true     : {TargetTrue}");
            //Console.WriteLine($"false    : {TargetFalse}");
            int numItems = Items.Count;
            for (int i = 0; i < numItems; i++)
            {
                Console.Write($"{Items.ElementAt(i)}, ");
            }
            Console.WriteLine();
        }

        public void InspectItems()
        {
            while (Items.Count > 0)
            {
                Inspections += 1;

                long item = Items.Dequeue();
                long worryLevel = PerformOperation(item);

                if (IsPart1)
                    worryLevel /= 3;
                else
                    worryLevel %= LCD;

                if (worryLevel % Divisor == 0)
                {
                    // throw to monkey x
                    //Console.WriteLine($"throwing {worryLevel} to monkey {TargetTrue}");
                    OutMonkeys.Enqueue(TargetTrue);
                    OutItems.Enqueue(worryLevel);
                }
                else
                {
                    // throw to monkey y
                    //Console.WriteLine($"throwing {worryLevel} to monkey {TargetFalse}");
                    OutMonkeys.Enqueue(TargetFalse);
                    OutItems.Enqueue(worryLevel);
                }
            }
        }
        
        public void AddItemToQueue(long item)
        {
            Items.Enqueue(item);
        }

        //public (int, long) ThrowItemToNextMonkey()
        //{
        //    int monkey = -1;
        //    long item = -1;

        //    if (OutMonkeys.Count > 0)
        //    { 
        //        monkey = OutMonkeys.Dequeue();
        //        item = OutItems.Dequeue();
        //    }

        //    return (monkey, item);
        //}

        private long PerformOperation(long item)
        {
            // parse operation
            string[] opWords = Operation.Split(' ');
            string sign = opWords[3];

            long operand;
            if (opWords[4].Trim() == "old")
                operand = item;
            else
                operand = Convert.ToInt64(opWords[4]);

            if (sign == "+")
            {
                return item + operand;
            }
            else//      if (sign == "*")
            {
                return item * operand;
            }
        }
    }
}
