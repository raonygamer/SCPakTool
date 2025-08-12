using CliFx;
using Saturn.PakTool.Commands;

namespace Saturn.PakTool
{
    public class Program
    {
        public const string Author = "ryd3v";

        static async Task<int> Main(string[] args)
        {
            Console.WriteLine($"Starting scpaktool...");
            Console.WriteLine($"Created by {Author}.");
            Console.WriteLine();

            return await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .Build()
                .RunAsync(args);
        }
    }
}
