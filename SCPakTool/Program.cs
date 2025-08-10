using System.CommandLine;
using System.IO;
using System.Text;

namespace SCPakTool
{
    using Common;
    using SCPakTool.Package.Content;
    using SCPakTool.Pak;
    using System.Drawing;
    using System.Globalization;

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
            
            Option<FileInfo> inputPak = new("--pak")
            {
                Description = "Specifies a .pak file for any operation."
            };

            Option<DirectoryInfo> outputDirectory = new("--out")
            {
                Description = "Specifies a output directory for any operation."
            };

            Option<string?> decryptHeaderKey = new("--header-key")
            {
                Description = "A string that specifies the key to be used to decrypt the header of the .pak."
            };

            Option<string?> decryptContentKey = new("--content-key")
            {
                Description = "A string that specifies the key to be used to decrypt the content of the .pak."
            };

            Command inspectCommand = new("inspect", "Inspects a .pak file specified by '--pak [file]'.");
            inspectCommand.Options.Add(inputPak);
            inspectCommand.Options.Add(decryptHeaderKey);
            inspectCommand.Options.Add(decryptContentKey);
            inspectCommand.SetAction(r => ExecuteInspectCommand(r).GetAwaiter().GetResult());
            rootCommand.Subcommands.Add(inspectCommand);

            Command unpackCommand = new("unpack", "Unpacks a .pak file specified by '--pak [file]' to the directory specified by '--out [directory]'.");
            unpackCommand.Options.Add(inputPak);
            unpackCommand.Options.Add(outputDirectory);
            unpackCommand.Options.Add(decryptHeaderKey);
            unpackCommand.Options.Add(decryptContentKey);
            unpackCommand.SetAction(r => ExecuteUnpackCommand(r).GetAwaiter().GetResult());
            rootCommand.Subcommands.Add(unpackCommand);

            var parseResult = rootCommand.Parse(args);
            return await parseResult.InvokeAsync();
        }

        public async Task<int> ExecuteUnpackCommand(ParseResult result)
        {
            if (result.GetValue<FileInfo>("--pak") is FileInfo pakFile && pakFile.Exists &&
                result.GetValue<DirectoryInfo>("--out") is DirectoryInfo outDir && outDir.Exists)
            {
                var headerKey = result.GetValue<string>("--header-key");
                var contentKey = result.GetValue<string>("--content-key");
                using var fs = pakFile.OpenRead();
                try
                {
                    var package = new PackageFile(fs, headerKey, contentKey);
                }
                catch (Exception e)
                {
#if DEBUG
                    throw;
#else
                    Log.Error(e.Message);
                    return -1;
#endif
                }
                return 0;
            }
            return -1;
        }

        public async Task<int> ExecuteInspectCommand(ParseResult result)
        {
            if (result.GetValue<FileInfo>("--pak") is FileInfo pakFile && pakFile.Exists)
            {
                var headerKey = result.GetValue<string>("--header-key");
                var contentKey = result.GetValue<string>("--content-key");
                using var fs = pakFile.OpenRead();
                try
                {
                    var package = new PackageFile(fs, headerKey, contentKey);
                    Log.Write($"Inspecting '{pakFile.FullName}'...");
                    Log.Write($"Survivalcraft 2 Package File:", ConsoleColor.DarkCyan);
                    Log.Write($" - Size: {(package.Size / 1.049e+6).ToString("F3", CultureInfo.InvariantCulture)} MiB", ConsoleColor.Green);
                    Log.Write($" - Content Count: {package.ContentMetadata.Count}", ConsoleColor.DarkYellow);
                    Log.Write($" - Contents:", ConsoleColor.DarkYellow);
                    foreach (var (name, content) in package.ContentMetadata)
                    {
                        Log.Write($"    - {content.Name}", ConsoleColor.White);
                        Log.Write($"       - Type: {content.Type}", ConsoleColor.DarkMagenta);
                        Log.Write($"       - Data Offset: 0x{content.Offset:X}", ConsoleColor.Green);
                        Log.Write($"       - Data Size: {(content.Size / 1024f).ToString("0.###", CultureInfo.InvariantCulture)} KiB", ConsoleColor.Green);
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    throw;
#else
                    Log.Error(e.Message);
                    return -1;
#endif
                }
                return 0;
            }
            return -1;
        } 
    }
}
