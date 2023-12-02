using Ornaments.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day2AdventOfCode;
[RegisterOrnament("Cube Conundrum", 2023, 2)]
internal partial class _2023Day2
{
    private List<ElfGame> Games { get; set; }

    public int RedConstraint { get; private set; }

    public int GreenConstraint { get; private set; }

    public int BlueConstraint { get; private set; }

    public _2023Day2(string[] input,int Red, int Green, int Blue)
    {
        Games = new List<ElfGame>();
        foreach (string line in input)
        {
            Games.Add(new ElfGame(line));
        }
        RedConstraint = Red;
        GreenConstraint = Green;
        BlueConstraint = Blue;
    }

    public int DoPart1()
    {
        int result = 0;
        foreach (ElfGame game in Games)
        {
            if (IsGamePossibleUnderConstraints(game))
            {
                result += game.Id;
            }
        }
        return result;
    }

    public int DoPart2()
    {
        int result = 0;
        foreach (ElfGame game in Games)
        {
            result += game.CalculateGamePower();
        }
        return result;
    }

    public int Calculate()
    {
        int result = Games[0].CalculateGamePower();
        return result;
    }

    private bool IsGamePossibleUnderConstraints(ElfGame game)
    {
        return game.IsPossibleUnderConstraints(RedConstraint,GreenConstraint,BlueConstraint);
    }

    private partial class ElfGame
    {
        public int Id { get; private set; }
        private List<Pull> Pulls { get; set; }

        public ElfGame(string input)
        {
            string[] split = Split(input);
            Id = ExtractId(split[0]);
            Pulls = CreatePulls(split[1]);
        }

        private string[] Split(string input)
        {
            input = input.Trim();
            return input.Split(':');
        }

        private int ExtractId(string input)
        {
            string id = input.Split(' ')[1];
            return int.Parse(id);
        }

        private string[] SplitPulls(string input)
        {
            string[] split = input.Split(';');
            return split;
        }

        private List<Pull> CreatePulls(string input)
        {
            string[] split = SplitPulls(input);
            List<Pull> result = new List<Pull>();
            foreach (string pull in split)
            {
                result.Add(CreatePull(pull));
            }

            return result;
        }

        [GeneratedRegex(@"\b\d+\s*(red)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex ExtractRed();
        [GeneratedRegex(@"\b\d+\s*(green)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex ExtractGreen();
        [GeneratedRegex(@"\b\d+\s*(blue)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex ExtractBlue();

        private Pull CreatePull(string input)
        {
            var red = ExtractRed().Matches(input);
            var green = ExtractGreen().Matches(input);
            var blue = ExtractBlue().Matches(input);
            Pull result = new Pull(ExtractValue(red), ExtractValue(green), ExtractValue(blue));
            return result;
        }

        private int ExtractValue(MatchCollection matches)
        {
            if (matches.Count == 0)
            {
                return 0;
            }

            string value = matches.First().Value;
            string[] splits = value.Split(' ');
            return int.Parse(splits[0]);
        }

        public bool IsPossibleUnderConstraints(int red, int green, int blue)
        {
            foreach (Pull pull in Pulls)
            {
                if (pull.Red > red || pull.Green > green || pull.Blue > blue)
                {
                    return false;
                }
            }

            return true;
        }

        private (int red, int green, int blue) GetMaximumValues()
        {
            int red = MaximumRed();
            int green = MaximumGreen();
            int blue = MaximumBlue();

            return (red, green, blue);
        }

        private int MaximumRed()
        {
            int result = int.MinValue;
            foreach (Pull pull in Pulls)
            {
                if (result < pull.Red)
                {
                    result = pull.Red;
                }
            }
            return result;
        }

        private int MaximumGreen()
        {
            int result = int.MinValue;
            foreach (Pull pull in Pulls)
            {
                if (result < pull.Green)
                {
                    result = pull.Green;
                }
            }
            return result;
        }

        private int MaximumBlue()
        {
            int result = int.MinValue;
            foreach (Pull pull in Pulls)
            {
                if (result < pull.Blue)
                {
                    result = pull.Blue;
                }
            }
            return result;
        }

        private int Power((int red,int green,int blue) minimal)
        {
            int result = minimal.red * minimal.green * minimal.blue;
            return result;
        }

        public int CalculateGamePower()
        {
            var minimal = GetMaximumValues();
            return Power(minimal);
        }

        private class Pull
        {
            public Pull(int red, int green, int blue)
            {
                Red = red;
                Green = green;
                Blue = blue;
            }

            public int Red { get; private set; }
            public int Green { get; private set; }
            public int Blue { get; private set; }
        }
    }
}
