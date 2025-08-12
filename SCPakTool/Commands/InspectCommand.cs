using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Saturn.PakTool.Pak;

namespace Saturn.PakTool.Commands
{
    [Command("inspect", Description = "Inspects a package file and prints it's contents.")]
    public class InspectCommand : ICommand
    {
        [CommandParameter(0, Description = "Specifies a .pak file to read from.", IsRequired = true, Name = "package")]
        public FileInfo File { get; set; } = default!;

        [CommandOption("header-key", Description = "Specifies a key to be used to decrypt the package header.")]
        public string? HeaderKey { get; set; } = null;

        [CommandOption("content-key", Description = "Specifies a key to be used to decrypt the package content.")]
        public string? ContentKey { get; set; } = null;

        public async ValueTask ExecuteAsync(IConsole console)
        {
            if (File is { Exists: true })
            {
                var package = new Package(File.FullName, new PackageEncryption(HeaderKey, ContentKey));
                console.WriteLine($"Inspecting '{File.Name}'...");
                console.WriteLine($"There are {package.Assets.Count} assets in the package.");
                foreach (var asset in package)
                {
                    console.WriteLine($"  Asset: '{asset.Meta.Name}' of type '{asset.Meta.Type}'.");
                }
            }
            else
            {
                throw new FileNotFoundException("Failed to find the specified .pak file!", File.FullName);
            }
            await ValueTask.CompletedTask;
        }
    }
}
