global using System.Collections.Immutable;
global using System.Text.RegularExpressions;
global using System.Text;
global using System.Diagnostics;
global using CommunityToolkit.HighPerformance;

namespace AdventOfCode;
public enum Mode
{
    Run,
    Upload,
    Display,
    Benchmark,
}

public static class Globals
{
    public static Mode CurrentRunMode { get; set; } = Mode.Run;
    public static bool IsTestInput { get; set; } = false;
}
