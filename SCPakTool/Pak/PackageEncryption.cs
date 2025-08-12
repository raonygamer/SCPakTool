using Saturn.PakTool.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saturn.PakTool.Pak
{
    public class PackageEncryption(string? header, string? content)
    {
        public readonly byte[]? Header = Encryption.GetBytes(header);
        public readonly byte[]? Content = Encryption.GetBytes(content);

        public string? GetHeaderString() => Encryption.GetString(Header);
        public string? GetContentString() => Encryption.GetString(Content);
        public bool HasHeaderKey() => Header is not null;
        public bool HasContentKey() => Content is not null;
    }
}
