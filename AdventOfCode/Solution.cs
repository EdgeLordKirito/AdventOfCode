namespace AdventOfCode;
internal abstract class Solution
{
    public Solution(string inputPath)
    {
        InputPath = inputPath;
    }
    public string InputPath { get; set; }
    public string[] Input { get; set; } = Array.Empty<string>();

    public abstract object DoPartOne();
    public abstract object DoPartTwo();
    public abstract void Run();
    public abstract void Init();
}
