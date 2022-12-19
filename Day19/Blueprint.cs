using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
//Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.
namespace Day19
{
    public class Blueprint
    {
        public int ID { get; set; }
        public int OreBots { get; set; }
        public int OreBotCost { get; set; }
        public int ClayBots { get; set; }
        public int ClayBotCost { get; set; }
        public int ObsBots { get; set; }
        public int ObsBotCostOre { get; set; }
        public int ObsBotCostClay { get; set; }
        public int GeoBots { get; set; }
        public int GeoBotCostOre { get; set; }
        public int GeoBotCostObs { get; set; }

        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geode { get; set; }

        public Blueprint(int iD, int oreBotCost, int clayBotCost, int obsBotCostOre, int obsBotCostClay, int geoBotCostOre, int geoBotCostObs)
        {
            ID = iD;
            OreBotCost = oreBotCost;
            ClayBotCost = clayBotCost;
            ObsBotCostOre = obsBotCostOre;
            ObsBotCostClay= obsBotCostClay;
            GeoBotCostOre = geoBotCostOre;
            GeoBotCostObs = geoBotCostObs;

            OreBots = 1;
            ClayBots = 0;
            ObsBots = 0;
            GeoBots = 0;

            Ore = 0;
            Clay = 0;
            Obsidian = 0;
            Geode = 0;
        }

        public override string ToString()
        {
            //return $"{ID},{OreBotCost},{ClayBotCost},{ObsBotCostOre},{ObsBotCostClay},{GeoBotCostOre},{GeoBotCostObs}";
            return $"{ID}: Ore({OreBots}):{Ore}  Cla({ClayBots}):{Clay}  Obs({ObsBots}):{Obsidian}  Geo({GeoBots}):{Geode}";
        }

        public void NextMinute()
        {
            bool orderOreBot = false;
            bool orderClayBot = false;
            bool orderObsBot = false;
            bool orderGeoBot = false;

            // build first
            if (Ore >= OreBotCost)
            {
                Ore -= OreBotCost;
                orderOreBot = true;
            }

            if (Ore >= ClayBotCost)
            {
                Ore -= ClayBotCost;
                orderClayBot = true;
            }
            
            if (Ore >= ClayBotCost)
            {
                Ore -= ClayBotCost;
                orderClayBot = true;
            }

            if (Ore >= ObsBotCostOre && Clay >= ObsBotCostClay)
            {
                Ore -= ObsBotCostOre;
                Clay -= ObsBotCostClay;
                orderObsBot = true;
            }

            if (Ore >= GeoBotCostOre && Obsidian >= GeoBotCostObs)
            {
                Ore -= GeoBotCostOre;
                Obsidian -= GeoBotCostObs;
                orderGeoBot = true;
            }


            Ore += OreBots;
            Clay += ClayBots;
            Obsidian += ObsBots;
            Geode += GeoBots;

            if (orderOreBot)
                OreBots++;
            if (orderClayBot)
                ClayBots++;
            if (orderObsBot)
                ObsBots++;
            if (orderGeoBot)
                GeoBots++;

        }

        public int GetQualityLevel()
        {
            return ID * Geode;
        }
    }
}
