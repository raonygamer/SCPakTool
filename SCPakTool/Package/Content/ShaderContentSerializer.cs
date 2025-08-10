using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Package.Content
{
    [ContentSerializer("Engine.Graphics.Shader")]
    public class ShaderContentSerializer : ContentSerializer
    {
        public string Vertex = string.Empty;
        public string Fragment = string.Empty;
        public Dictionary<string, string> Macros = [];

        public override IEnumerable<string> GetFileExtension()
        {
            yield return "vert";
            yield return "frag";
            yield return "def";
        }

        public override void Read(Stream stream)
        {
            Macros.Clear();
            using var reader = new BinaryReader(stream);
            Vertex = reader.ReadString();
            Fragment = reader.ReadString();
            int macroCount = reader.ReadInt32();
            for (var i = 0; i < macroCount; i++)
            {
                Macros.Add(reader.ReadString(), reader.ReadString());
            }
        }

        public override void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream);
            writer.Write(Vertex);
            writer.Write(Fragment);
            writer.Write(Macros.Count);
            foreach (var (name, value) in Macros)
            {
                writer.Write(name);
                writer.Write(value);
            }
        }

        public override void Write(FileStream[] fileStreams)
        {
            if (fileStreams.Length < 3)
                return;
            using var vertWriter = new BinaryWriter(fileStreams[0]);
            using var fragWriter = new BinaryWriter(fileStreams[1]);
            using var defWriter = new BinaryWriter(fileStreams[2]);
            vertWriter.Write(Encoding.UTF8.GetBytes(Vertex));
            fragWriter.Write(Encoding.UTF8.GetBytes(Fragment));
            string defText = string.Empty;
            foreach (var (name, value) in Macros)
            {
                defText += $"{name}={value};";
            }
            defWriter.Write(Encoding.UTF8.GetBytes(defText));
        }
    }
}
