using System.Text;
using System.Text.RegularExpressions;


namespace Day19
{
    public class Blueprint
    {
        public int ID { get; set; }

        private List<List<(int, int)>> bp;
        private int[] botCount;
        private int[] material;
        private int[] maxSpend;
        private Dictionary<(int, int, int, int, int, int, int, int, int), int> cache;


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

            maxSpend = new int[3];       
            maxSpend[0] = bp.Max(x => x[0].Item1);  // max ore spend per turn
            maxSpend[1] = bpNums[4];                // max clay spend per turn
            maxSpend[2] = bpNums[6];                // max obsidian spend per turn

            botCount = new int[4];
            botCount[0] = 1;                        // start with 1 ore bot

            material = new int[4];                  // ore, clay, obsidian, geode we've collected

            cache = new();
        }

        // utility function to print blueprint
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

        /// <summary>
        /// Part 1: multiply blueprint ID and maximum geodes
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public int GetQualityLevel(int minutes)
        {
            int quality = ID * DFS(bp, maxSpend, cache, minutes, botCount, material);
            
            Console.WriteLine($"quality:{quality}");
            return quality;
        }

        /// <summary>
        /// Part 2: return maximum geodes
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public int GetMaximumGeodes(int minutes)
        {
            int maxGeodes = DFS(bp, maxSpend, cache, minutes, botCount, material);

            Console.WriteLine($"maxGeodes:{maxGeodes}");
            return maxGeodes;
        }

        /// <summary>
        /// Create cache key from time remaining, bot array, and material array
        /// </summary>
        /// <param name="time"></param>
        /// <param name="bots"></param>
        /// <param name="amt"></param>
        /// <returns></returns>
        private (int, int, int, int, int, int, int, int, int) CacheKey(int time, int[] bots, int[] amt)
        {
            return (time, bots[0], bots[1], bots[2], bots[3], amt[0], amt[1], amt[2], amt[3]);
        }

        /// <summary>
        /// Utility to print arrays
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private string Arr2Str(int[] arr)
        {
            string output = "[";

            for (int i = 0; i < arr.Length; i++)
                output += (i != 0) ? $", {arr[i]}" : $"{arr[i]}";

            output += "]";
            
            return output;
        }

        /// <summary>
        /// Depth first search
        /// </summary>
        /// <param name="bp"></param>
        /// <param name="maxSpend"></param>
        /// <param name="cache"></param>
        /// <param name="time"></param>
        /// <param name="bots"></param>
        /// <param name="amt"></param>
        /// <returns></returns>
        private int DFS(List<List<(int, int)>> bp, int[] maxSpend, 
                    Dictionary<(int, int, int, int, int, int, int, int, int), int> cache, 
                    int time, int[] bots, int[] amt)
        {
            //Console.WriteLine("--- entering DFS ---");
            // out of time
            if (time == 0)
                return amt[3];

            // don't recalculate if we've done this scenario before
            // lookup in cache and return its value
            //var key = (time, bots, amt);
            var key = CacheKey(time, bots, amt);
            if (cache.ContainsKey(key))
                return cache[key];

            // if we do nothing, this is the number of geodes at the end
            int maxVal = amt[3] + (bots[3] * time);

            // how many geodes can we get if we build each type of bot
            for (int botType = 0; botType < bp.Count; botType++)
            {
                // skip this bot type if we already have enough of these bots
                if (botType != 3 && bots[botType] >= maxSpend[botType])
                    continue;

                int wait = 0;

                // for each bot recipe
                for (int i = 0; i < bp[botType].Count; i++)
                {
                    var resource = bp[botType][i];

                    int resourceAmt = resource.Item1;
                    int resourceType = resource.Item2;

                    //Console.WriteLine($"ramt:{resourceAmt} rtype:{resourceType}");
                    if (bots[resourceType] == 0)
                    {
                        // in case we don't have any bots of this type
                        //Console.WriteLine($"no bots of rtype:{resourceType}, breaking");
                        break;
                    }

                    //wait = Math.Max(wait, -(-(resourceAmt - amt[resourceType]) / bots[resourceType]));
                    wait = Math.Max(wait, (int) Math.Ceiling(((decimal) resourceAmt - amt[resourceType]) / bots[resourceType]));
                    //wait = ((resourceAmt - amt[resourceType]) + bots[resourceType] - 1) / bots[resourceType];

                    // only do this part if it's the last iteration of the outer for loop
                    if (i == bp[botType].Count - 1)
                    {
                        int remTime = time - wait - 1;
                        
                        if (remTime <= 0)
                            continue;

                        //Console.WriteLine($"bots:{Arr2Str(bots)}");
                        //Console.WriteLine($"amt:{Arr2Str(amt)}");

                        // create local copies of bots and amount arrays to pass to recursive call
                        int[] bots_ = new int[4];
                        int[] amt_ = new int[4];
                        for (int j = 0; j < 4; j++)
                        {
                            int x = amt[j];
                            int y = bots[j];
                            bots_[j] = y;
                            amt_[j] = (x + y * (wait + 1));
                        }

                        //Console.WriteLine($"bots_:{Arr2Str(bots_)}");
                        //Console.WriteLine($"amt_:{Arr2Str(amt_)} adjusted for wait");

                        // build new bot, deduct resources and increment bot count
                        foreach (var recipe in bp[botType])
                        {
                            amt_[recipe.Item2] -= recipe.Item1;
                        }
                        bots_[botType] += 1;

                        // check minimum
                        for (int j = 0; j < 3; j++)
                            amt_[j] = Math.Min(amt_[j], maxSpend[j] * remTime);
                        
                        maxVal = Math.Max(maxVal, DFS(bp, maxSpend, cache, remTime, bots_, amt_));
                    }
                }
            }
            
            // add this scenario to the cache
            cache.Add(key, maxVal);
            return maxVal;
        }

    }
}
