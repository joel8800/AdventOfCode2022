Console.WriteLine("Day02: Rock Paper Scissors");

// A = rock         X = lose
// B = paper        Y = draw
// C = scissors     Z = win
//
// score = shape selected (1, 2, 3) + outcome (0, 3, 6)

string[] input = File.ReadAllLines(@"input.txt");

int scorePt1 = 0;
int scorePt2 = 0;

foreach (string line in input)
{
    string[] plays = line.Split(' ');

    scorePt1 += PlayRound(plays[0], plays[1]);
    scorePt2 += PlayForResult(plays[0], plays[1]);
}

Console.WriteLine($"Part1 score: {scorePt1}");
Console.WriteLine($"Part2 score: {scorePt2}");


int PlayRound(string opponent, string player)
{
    int score = 0;

    if (player == "X") score = 1;
    else if (player == "Y") score = 2;
    else if (player == "Z") score = 3;

    switch (opponent)
    {
        case "A":                   // rock
            if (player == "X")      // vs rock
                score += 3;
            if (player == "Y")      // vs paper
                score += 6;
            if (player == "Z")      // vs scissor
                score += 0;
            break;
        case "B":                   // paper
            if (player == "X")      // vs rock
                score += 0;
            if (player == "Y")      // vs paper
                score += 3;
            if (player == "Z")      // vs scissor
                score += 6;
            break;
        case "C":                   // scissors
            if (player == "X")      // vs rock
                score += 6;
            if (player == "Y")      // vs paper
                score += 0;
            if (player == "Z")      // vs scissor
                score += 3;
            break;
    }

    return score;
}

int PlayForResult(string opponent, string player)
{
    int score = 0;

    if (player == "X") score = 0;       // X must lose
    else if (player == "Y") score = 3;  // Y must draw
    else if (player == "Z") score = 6;  // Z must win

    switch (opponent)
    {
        case "A":                   // rock
            if (player == "X")      // x must lose, scissor
                score += 3;
            if (player == "Y")      // y must draw, rock
                score += 1;
            if (player == "Z")      // z must win, paper
                score += 2;
            break;
        case "B":                   // paper
            if (player == "X")      // x must lose, rock
                score += 1;
            if (player == "Y")      // y must draw, paper
                score += 2;
            if (player == "Z")      // z must win, scissor
                score += 3;
            break;
        case "C":                   // scissor
            if (player == "X")      // x must lose, paper
                score += 2;
            if (player == "Y")      // y must draw, scissor
                score += 3;
            if (player == "Z")      // z must win, rock
                score += 1;
            break;
    }

    return score;
}