namespace AdventOfCode;
internal class Globals
{
    public static readonly string RootPath = AppDomain.CurrentDomain.BaseDirectory;
    public static readonly string InputPath = Path.Combine(RootPath, "Input");
    public static readonly string OutputPath = Path.Combine(RootPath, "Output");
    public static readonly string Input2023 = Path.Combine(InputPath, "2023");
    public static readonly string Output2023 = Path.Combine(OutputPath, "2023");
}
