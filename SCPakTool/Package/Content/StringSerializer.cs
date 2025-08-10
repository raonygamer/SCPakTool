using SCPakTool.Pak;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Package.Content
{
    [ContentSerializer("System.String")]
    public class StringSerializer : ContentSerializer
    {
        public string Value = string.Empty;

        public StringSerializer(ContentDescriptor descriptor) : base(descriptor)
        {
        }

        public override IEnumerable<string> GetFileExtensions() 
        { 
            yield return "txt"; 
        }

        public override void ReadFromPackage(PackageFile package)
        {
            using var reader = package.ReadContentData(ContentDescriptor);
            if (reader is null)
                return;
            Value = reader.ReadString();
        }

        public override void WriteToPackage(PackageFile package)
        {
            
        }
    }
}
