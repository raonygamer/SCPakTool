using SCPakTool.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Pak
{
    public class PackageFile : IDisposable
    {
        public const int ContentTableOffset = 16;
        public static readonly string ExpectedSignature = "PK2\0";

        public EncryptedStream HeaderStream { get; private set; }
        public EncryptedStream ContentStream { get; private set; }

        public BinaryReader HeaderReader { get; private set; }
        public BinaryReader ContentReader { get; private set; }

        public byte[] Signature { get; private set; }
        public long ContentOffset { get; private set; }
        public int ContentCount { get; private set; }

        public List<ContentDescriptor> ContentTable { get; private set; } = [];

        public PackageFile(Stream dataStream, string? headerKey = null, string? contentKey = null)
        {
            var signatureBytes = new byte[4];
            dataStream.Read(signatureBytes, 0, 4);
            dataStream.Position = 0;
            if (Encoding.UTF8.GetString(signatureBytes) is string sig && sig != ExpectedSignature)
                throw new FormatException($"Failed to read .pak file, the signature didn't match what was expected. Expected '{ExpectedSignature}', got '{sig}'!");
            HeaderStream = new EncryptedStream(dataStream, headerKey is not null ? Encoding.UTF8.GetBytes(headerKey) : null, 4);
            HeaderReader = new BinaryReader(HeaderStream);
            using (new ScopedCursor(HeaderStream, 0))
            {
                Signature = HeaderReader.ReadBytes(4);
                ContentOffset = HeaderReader.ReadInt64();
                ContentCount = HeaderReader.ReadInt32();
            }
            ContentStream = new EncryptedStream(dataStream, contentKey is not null ? Encoding.UTF8.GetBytes(contentKey) : null, ContentOffset);
            ContentReader = new BinaryReader(ContentStream);
            ReadContentTable();
        }

        private void ReadContentTable()
        {
            ContentTable.Clear();
            using (new ScopedCursor(HeaderStream, ContentTableOffset))
            {
                for (var i = 0; i < ContentCount; i++)
                {
                    ContentTable.Add(new ContentDescriptor(HeaderReader));
                }
            }
        }

        public byte[]? ReadAssetBytes(ContentDescriptor assetEntry)
        {
            var entry = ContentTable.Find(o => o.Name == assetEntry.Name);
            if (entry == null)
                return null;
            var last = ContentStream.Position;
            ContentStream.Position = ContentOffset + entry.Offset;
            var bytes = ContentReader.ReadBytes((int)entry.Size);
            ContentStream.Position = last;
            return bytes;
        }

        public override string ToString()
        {
            var text = $"Survivalcraft 2 Package File - {(HeaderStream.BaseStream.Length / 1.049e+6).ToString("F3", CultureInfo.InvariantCulture)} MB - {ContentCount} assets\n";
            for (var i = 0; i < ContentTable.Count; i++)
            {
                text += "  > " + ContentTable[i].ToString() + "\n";
            }
            return text;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            HeaderStream.Dispose();
            HeaderReader.Dispose();
            ContentStream.Dispose();
            ContentReader.Dispose();
        }
    }
}
