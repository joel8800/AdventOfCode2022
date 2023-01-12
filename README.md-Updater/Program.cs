﻿using System.Text.RegularExpressions;

// Shamelessly stole this code from Troy Zhang (https://github.com/ivylongbow/AoC-2022-Csharp/blob/main/README.md-Updater/Program.cs)
// and adapted the parsing to match my project naming conventions


string[] solution = File.ReadAllLines("../../../../AdventOfCode2022.sln");

Dictionary<int, string> projects = new();
Dictionary<int, string> titles = new();

foreach (string s in solution)
{
    if (s.StartsWith("Project("))
    {
        Match proj = Regex.Match(s, @"Day\d\d");   // grab project names that start with "Day" followed by 2-digit number
        if (proj.Success)
        {
            string dayProj = proj.Value;   //Groups[0].Value;  //.Substring(3, 2);

            int day = Convert.ToInt32(dayProj.Substring(3, 2));
            projects.Add(day, dayProj);

            // get title of puzzle for this day
            string url = $"https://adventofcode.com/2022/day/{day}";
            string title = string.Empty;

            Console.WriteLine($"Getting title for day {day}");

            HttpClient client = new();
            try
            {
                // load the puzzle page for the day
                string responseBody = await client.GetStringAsync(url);

                // be a good net citizen
                Thread.Sleep(5000);

                // find the title
                Match m = Regex.Match(responseBody, @"--- Day (.*) ---");
                if (m.Success)
                {
                    string fullTitle = m.Groups[0].Value;
                    string[] titleParts = fullTitle.Split(':', StringSplitOptions.TrimEntries);
                    title = titleParts[1].Substring(0, titleParts[1].Length - 4);
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception caught");
                Console.WriteLine($"Message :{e.Message}");
            }

            if (title != string.Empty)
                titles.Add(day, title);
        }
    }
    else if (s.StartsWith("Global"))    // end the Parsing process.
        break;
}


int DayProgress = projects.Count;

// Formatting the output string for file "Readme.md"
List<string> ReadMe = new()
            {
                "# Advent of Code 2022 in C#",
                "- This fancy readme style is inspired by [Marcus Shu](https://github.com/shulkx/advent-of-code/tree/main/adventofcode2022).",
                "- This README.md file is generated by a program inspired by [Troy Zhang's updater](https://github.com/ivylongbow/AoC-2022-Csharp/blob/main/README.md-Updater/Program.cs).",
                "",
                $"## Progression:  ![Progress](https://progress-bar.dev/{DayProgress}/?scale=25&title=projects&width=240&suffix=/25)",
                "",
                "| Day                                                          | C#                            | Stars |  Solution Description |",
                "| ------------------------------------------------------------ | ----------------------------- | ----- | -------------------- |"
            };


for (int i = 1; i <= DayProgress; i++)
{
    ReadMe.Add($"| [Day {i:D02}:  {titles[i]}](https://adventofcode.com/2022/day/{i}) | [Solution](./{projects[i]}/Program.cs) | :star::star: |");
}

foreach (string s in ReadMe)
    Console.WriteLine(s);

File.WriteAllLines("../../../../README.md", ReadMe);
