﻿namespace AdventOfCode;
internal class Globals
{
    public static readonly string RootPath = AppDomain.CurrentDomain.BaseDirectory;
    public static readonly string InputPath = Path.Combine(RootPath, "Input");
    public static readonly string Input2023 = Path.Combine(InputPath, "2023");
}