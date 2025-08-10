using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Pak
{
    public class ContentDescriptor
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public long Offset { get; set; } = -1;
        public long Size { get; set; } = -1;

        public ContentDescriptor(BinaryReader reader)
        {
            var stream = reader.BaseStream;
            if (stream.Length - stream.Position < 18)
                throw new InvalidDataException("Failed to read asset entry because there were less bytes than what was needed.");
            Name = reader.ReadString();
            Type = reader.ReadString();
            Offset = reader.ReadInt64();
            Size = reader.ReadInt64();
        }

        public static ContentDescriptor Read(BinaryReader reader)
        {
            return new ContentDescriptor(reader);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Type);
            writer.Write(Offset);
            writer.Write(Size);
        }

        public override string ToString()
        {
            return $"Name: {Name} | Type: {Type} | Offset: 0x{Offset:X} | Size: {(Size / 1.049e+6).ToString("F3", CultureInfo.InvariantCulture)} MB";
        }
    }
}
