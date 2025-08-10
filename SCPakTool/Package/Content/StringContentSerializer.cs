using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Package.Content
{
    [ContentSerializer("System.String")]
    public class StringContentSerializer : ContentSerializer
    {
        public string Value = string.Empty;

        public override string GetFileExtension() => "txt";
        public override void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream);
            Value = reader.ReadString();
        }

        public override void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream);
            writer.Write(Value);
        }

        public override void Write(FileStream fileStream)
        {
            using var writer = new BinaryWriter(fileStream);
            writer.Write(Encoding.UTF8.GetBytes(Value));
        }
    }
}
