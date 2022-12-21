using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    public class Monkey
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Operation { get; set; }
        public long Value { get; set; }

        public string lName;
        public string rName;
        public string op;

        public Monkey(int id, string name, long value)
        {
            ID = id;
            Name = name;
            Value= value;
            Operation = string.Empty;
            lName = string.Empty;
            rName = string.Empty;
            op = string.Empty;
        }

        public Monkey(int id, string name, string operation)
        {
            ID = id; 
            Name = name; 
            Operation = operation;
            Value = -99;

            string[] opTokens = Operation.Trim().Split(' ');
            lName = opTokens[0];
            rName = opTokens[2];
            op = opTokens[1];
        }
    }
}
