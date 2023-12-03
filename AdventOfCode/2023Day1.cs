using System.Text.RegularExpressions;

namespace AdventOfCode;
internal  sealed partial class _2023Day1 : Solution
{
    private static Dictionary<string, int> _lookup = new Dictionary<string, int>
    {
        { "zero" , 0 },
        { "one"  , 1 },
        { "two"  , 2 },
        { "three", 3 },
        { "four" , 4 },
        { "five" , 5 },
        { "six"  , 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine" , 9 },
        { "0"    , 0 },
        { "1"    , 1 },
        { "2"    , 2 },
        { "3"    , 3 },
        { "4"    , 4 },
        { "5"    , 5 },
        { "6"    , 6 },
        { "7"    , 7 },
        { "8"    , 8 },
        { "9"    , 9 },
    };

    public _2023Day1(string path): base(path)
    {
    }

    [GeneratedRegex("[0-9]|one|two|three|four|five|six|seven|eight|nine", RegexOptions.Compiled)]
    private static partial Regex ExtractNumbers();

    [GeneratedRegex("[0-9]|one|two|three|four|five|six|seven|eight|nine", RegexOptions.Compiled | RegexOptions.RightToLeft)]
    private static partial Regex ExtractNumbersRtl();

    //TODO: Method does not work properly in case of eightwothree it returns only 83 not 823
    public static int ExtractCalibrationValue(string s, bool partTwo)
    {
        var matchesLeft = ExtractNumbers().Matches(s);
        var matchesRight = ExtractNumbersRtl().Matches(s);
        int result = 0;

        if (matchesLeft.Count > 0)
        {
            var a = _lookup[matchesLeft.First(x => partTwo || int.TryParse(x.Value, out var _)).Value];
            var b = _lookup[matchesRight.First(x => partTwo || int.TryParse(x.Value, out var _)).Value];
            result = a * 10 + b;
        }
        return result;
    }

    public override object DoPartOne()
    {
        string[] input = new string[Input.Length];
        for (int i = 0; i < Input.Length; i++)
        {
            input[i] = new string(Input[i]);
        }
        return input
            .Select(s => ExtractCalibrationValue(s, false))
            .Sum();
    }

    public override object DoPartTwo()
    {
        string[] input = new string[Input.Length];
        for (int i = 0; i < Input.Length; i++)
        {
            input[i] = new string(Input[i]);
        }
        return input
            .Select(s => ExtractCalibrationValue(s, true))
            .Sum();
    }

    public override void Init()
    {
        FileInfo info = new FileInfo(InputPath);
        if (!info.Exists)
        {
            return;
        }
        string[] lines = File.ReadAllLines(InputPath);
        Input = lines;
    }

    public override void Run()
    {
        Init();
        Console.WriteLine($"2023 Day 1");
        Console.WriteLine($"Part one: {DoPartOne()}");
        Console.WriteLine($"Part two: {DoPartTwo()}");
    }
}