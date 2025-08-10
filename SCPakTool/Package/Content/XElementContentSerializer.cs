using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Package.Content
{
    [ContentSerializer("System.Xml.Linq.XElement")]
    public class XElementContentSerializer : ContentSerializer
    {
        public string Value = string.Empty;

        public override IEnumerable<string> GetFileExtension()
        {
            yield return "xml";
        }

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

        public override void Write(FileStream[] fileStreams)
        {
            if (fileStreams.Length == 0)
                return;
            using var writer = new BinaryWriter(fileStreams[0]);
            writer.Write(Encoding.UTF8.GetBytes(Value));
        }
    }
}
