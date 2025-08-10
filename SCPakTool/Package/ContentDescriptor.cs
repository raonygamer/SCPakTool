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
        public readonly string Name = string.Empty;
        public readonly string Type = string.Empty;
        public readonly long Offset = -1;
        public readonly long Size = -1;

        public ContentDescriptor(string name, string type, long offset = -1, long size = -1)
        {
            Name = name;
            Type = type;
            Offset = offset;
            Size = size;
        }
    }
}
