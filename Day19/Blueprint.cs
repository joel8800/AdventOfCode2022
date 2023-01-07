using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


//Blueprint 1: Each ore robot bpNums 4 ore. Each clay robot bpNums 2 ore. Each obsidian robot bpNums 3 ore and 14 clay. Each geode robot bpNums 2 ore and 7 obsidian.
//Blueprint 2: Each ore robot bpNums 2 ore. Each clay robot bpNums 3 ore. Each obsidian robot bpNums 3 ore and 8 clay. Each geode robot bpNums 3 ore and 12 obsidian.
namespace Day19
{
    public class Blueprint
    {
        public int ID { get; set; }

        private List<List<(int, int)>> bp;
        private int[] botCount;
        private int[] material;
        private int[] maxSpend;
        private Dictionary<(int, int[], int[]), int> cache;


        public Blueprint(string line)
        {
            // grab all the numbers in the blueprint line
            MatchCollection mc = Regex.Matches(line, @"\d+");

            List<int> bpNums = new();
            for (int i = 0; i < mc.Count; i++)
                bpNums.Add(Convert.ToInt32(mc[i].Value));

            ID = bpNums[0];

            bp = new()      // blueprint = bot recipes, these vary per blueprint
            {
                new() { (bpNums[1], 0) },                    // ore bot = x ore
                new() { (bpNums[2], 0) },                    // clay bot = x ore
                new() { (bpNums[3], 0), (bpNums[4], 1) },    // obsidian bot = x ore + y clay
                new() { (bpNums[5], 0), (bpNums[6], 2) }     // geode bot = x ore + y obsidian
            };

            maxSpend = new int[] { 0, 0, 0 };       // ore, clay, obsidian to spend per turn
            botCount = new int[] { 1, 0, 0, 0 };    // ore, clay, obsidian, geode bots that we have
            material = new int[] { 0, 0, 0, 0 };    // ore, clay, obsidian, geode we've collected
            cache = new();
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append($"ID:{ID} ");
            
            // type(bot recipe):numBots:quality
            sb.Append($"Ore({bp[0][0].Item1}):{botCount[0]}:{material[0]} ");
            sb.Append($"Cly({bp[1][0].Item1}):{botCount[1]}:{material[1]} ");
            sb.Append($"Obs({bp[2][0].Item1},{bp[2][1].Item1}):{botCount[2]}:{material[2]} ");
            sb.Append($"Geo({bp[3][0].Item1},{bp[3][1].Item1}):{botCount[3]}:{material[3]}");
            
            return sb.ToString();
        }

        public int GetQualityLevel()
        {
            // call dfs function
            int quality = DFS(bp, maxSpend, cache, 24, botCount, material);
            Console.WriteLine($"quality:{quality}");
            return ID * quality;
        }

        private int DFS(List<List<(int, int)>> bp, int[] maxSpend, 
                    Dictionary<(int, int[], int[]), int> cache, 
                    int time, int[] bots, int[] amounts)
        {
            Console.WriteLine("--- entering DFS ---");
            Console.WriteLine($"time:{time}");
            // out of time
            if (time == 0)
            {
                Console.WriteLine($"time:{time} amt[3]:{amounts[3]}");
                return amounts[3];
            }

            // don't recalculate if done before
            var key = (time, bots, amounts);
            Console.WriteLine(key);
            if (cache.ContainsKey(key))
            {
                Console.WriteLine($"{key} in cache");
                return cache[key];
            }

            int maxVal = amounts[3] + bots[3] * time;
            Console.WriteLine($"maxval:{maxVal} = amd[3]:{amounts[3]} + bots[3]:{bots[3]} * time:{time}");

            Console.WriteLine("----------");
            for (int i = 0; i < bp.Count; i++)
            {
                if (i != 3 && bots[i] >= maxSpend[i])
                    continue;

                int wait = 0;
                bool brk = false;

                // for each recipe
                foreach (var recipe in bp[i])
                {
                    if (bots[recipe.Item2] == 0)
                    {
                        brk = true;
                        break;
                    }
                    wait = Math.Max(wait, -(-(recipe.Item1 - amounts[recipe.Item2]) / bots[recipe.Item2]));
                }

                if (brk)
                {
                    int remTime = time - wait - 1;
                    if (remTime <= 0)
                        continue;

                    int[] bots_ = bots;
                    //[x + y * (wait + 1) for x, y in zip(amt, bots)]
                    int[] amt_ = new int[4];
                    for (int j = 0; j < 4; j++)
                    {
                        int x = amounts[j];
                        int y = bots[j];
                        amt_[j] = (x + y * (wait + 1));
                    }
                    
                    foreach (var recipe in bp[i])
                            amt_[recipe.Item2] -= recipe.Item1;
                    
                    bots_[i] += 1;

                    for (int j = 0; j < 3; j++)
                        amt_[j] = Math.Min(amt_[j], maxSpend[j] * remTime);

                    maxVal = Math.Max(maxVal, DFS(bp, maxSpend, cache, remTime, bots_, amt_));
                }
            }
            Console.WriteLine("----------");
            cache.Add(key, maxVal);
            Console.WriteLine($"--- returning {maxVal} ---");
            return maxVal;
        }

    }
}
