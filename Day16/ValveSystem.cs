using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class ValveSystem
    {
        public List<Valve> Valves { get; set; }

        private int released;


        public ValveSystem()
        {
            Valves = new();
            released = 0;
        }

        public void Add(Valve v)
        {
            Valves.Add(v);
        }

        public void ConnectTunnels()
        {
            foreach (Valve v in Valves)
            {
                foreach (string adjName in v.AdjIDs)
                {
                    Console.WriteLine($"connecting {v.ID} to {adjName}");
                    Valve target = Valves.First(x => x.ID == adjName);
                    v.AdjNodes.Add(target);
                }
            }
        }

        public int GetReleasedPressure()
        {
            int released = 0;

            foreach (Valve v in Valves)
            {
                if (v.OnOff == true)
                    released += v.Rate;
            }

            return released;
        }

        public void ResetSystem()
        {
            released = 0;
            foreach (Valve v in Valves)
                v.OnOff = false;
        }


        //public int DFS(Valve v)
        //{


        //}

    }
}
