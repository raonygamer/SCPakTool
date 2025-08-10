using SCPakTool.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Pak
{
    public class PackageFile
    {
        public const int ContentMetadataOffset = 16;
        public static readonly string ExpectedSignature = "PK2\0";

        public readonly long Size = -1;
        public readonly Dictionary<string, ContentDescriptor> ContentMetadata = [];
        public readonly Dictionary<string, byte[]> ContentData = [];

        public PackageFile(Stream dataStream, string? headerKey = null, string? contentKey = null)
        {
            Size = dataStream.Length;
            var signatureBytes = new byte[4];
            dataStream.Read(signatureBytes, 0, 4);
            dataStream.Position = 0;
            if (Encoding.UTF8.GetString(signatureBytes) is string sig && sig != ExpectedSignature)
                throw new FormatException($"Failed to read .pak file, the signature didn't match what was expected. Expected '{ExpectedSignature}', got '{sig}'!");
            using var headerStream = new EncryptedStream(dataStream, headerKey is not null ? Encoding.UTF8.GetBytes(headerKey) : null, 4);
            using var headerReader = new BinaryReader(headerStream);
            var contentOffset = 0L;
            var contentCount = 0;
            using (new ScopedCursor(headerStream, 0))
            {
                var _ = headerReader.ReadBytes(4);
                contentOffset = headerReader.ReadInt64();
                contentCount = headerReader.ReadInt32();
            }
            var contentStream = new EncryptedStream(dataStream, contentKey is not null ? Encoding.UTF8.GetBytes(contentKey) : null, contentOffset);
            var contentReader = new BinaryReader(contentStream);
            ReadContents(headerReader, contentReader, contentOffset, contentCount);
        }

        private void ReadContents(BinaryReader headerReader, BinaryReader contentReader, long contentOffset, int contentCount)
        {
            ContentMetadata.Clear();
            ContentData.Clear();
            using (new ScopedCursor(headerReader, ContentMetadataOffset))
            {
                for (var i = 0; i < contentCount; i++)
                {
                    var name = headerReader.ReadString();
                    var type = headerReader.ReadString();
                    var offset = headerReader.ReadInt64();
                    var size = headerReader.ReadInt64();
                    ContentMetadata.Add(name, new ContentDescriptor(name, type, offset, size));
                    byte[] data;
                    using (new ScopedCursor(contentReader, contentOffset + offset))
                    {
                        data = contentReader.ReadBytes((int)size);
                        ContentData.Add(name, data);
                    }
                }
            }
        }

        public BinaryReader? ReadContentData(ContentDescriptor descriptor)
        {
            if (!ContentData.TryGetValue(descriptor.Name, out var data))
                return null;
            return new BinaryReader(new MemoryStream(data));
        }

        public byte[]? GetBytesForContentData(ContentDescriptor descriptor)
        {
            if (!ContentData.TryGetValue(descriptor.Name, out var data))
                return null;
            return data;
        }
    }
}
