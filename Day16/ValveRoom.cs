using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class ValveRoom
    {
        public string ID { get; set; }
        public bool OnOff { get; set; }
        public int Rate { get; set; }
        public List<ValveRoom> AdjNodes { get; set; }
        public List<string> AdjIDs { get; set; }

        public ValveRoom(string name, string flowRate)
        {
            ID = name;
            OnOff = false;
            Rate = Convert.ToInt32(flowRate);
            AdjNodes = new();
            AdjIDs = new();
        }

        public void AddAdjacents(string adjString)
        {
            string justTunnels;
            string[] adjNames;

            if (adjString.StartsWith('s'))
                justTunnels = adjString.Substring(2);
            else
                justTunnels = adjString.Substring(1);

            adjNames = justTunnels.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string adjName in adjNames)
            {
                AdjIDs.Add(adjName);
            }
        }

        
        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append($"{ID}:{Rate}:[ ");
            foreach (string adjName in AdjIDs)
            {
                sb.Append($"{adjName} ");
            }
            sb.Append(']');

            return sb.ToString();
        }
    }
}
