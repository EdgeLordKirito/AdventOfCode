using Day2AdventOfCode;

namespace AdventOfCode;

internal class Program
{
    static void Main(string[] args)
    {
        new _2023Day1(Path.Combine(Globals.Input2023, "Day1.txt")).Run();
        new _2023Day2(Path.Combine(Globals.Input2023, "Day2.txt")).Run();
    }
}