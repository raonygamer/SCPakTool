using Common;
using System.CommandLine;
using System.IO;
using System.Text;

namespace SCPakTool
{
    public class Program
    {
        private static Program? _instance;
        public static Program Instance => _instance ??= new Program();
        static int Main(string[] args) => Instance.Start(args).GetAwaiter().GetResult();

        public const string Version = "1.0.0";
        public const string Author = "ryd3v";

        public async Task<int> Start(string[] args)
        {
            Log.Trace($"Starting SCPakTool v{Version}...");
            Log.Trace($"Created by {Author}.");
            Log.Write();
            RootCommand rootCommand = new("A command-line tool for unpacking, repacking, and inspecting Survivalcraft 2 .pak files.");
            Option<FileInfo> inspectOption = new("--inspect", "-ins")
            {
                Description = "Specifies if the program should inspect the .pak file."
            };
            rootCommand.Options.Add(inspectOption);
            rootCommand.SetAction(result => ExecuteCommandAction(result).GetAwaiter().GetResult());
            var parseResult = rootCommand.Parse(args);
            return await parseResult.InvokeAsync();
        }

        public async Task<int> ExecuteCommandAction(ParseResult result)
        {
            if (result.GetValue<FileInfo>("--inspect") is FileInfo info)
            {
                using var fs = info.OpenRead();
                using var reader = new BinaryReader(fs);
                string signature = Encoding.UTF8.GetString(reader.ReadBytes(4));
                if (signature != "PK2\0")
                {
                    Log.Error($"Failed to inspect '{info.Name}' because it was not a Survivalcraft 2 .pak file!");
                    return -1;
                }
                //TODO: code to inspect and read shit
                return 0;
            }
            return -1;
        } 
    }
}
