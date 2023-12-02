using Ornaments.App;

namespace AdventOfCode;

internal class Program
{
    static async Task Main(string[] args)
    {
        await OrnamentsApp.CreateDefault().RunAsync(args);
    }
}
