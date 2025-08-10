using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Package.Content
{
    public class ContentSerializerAttribute : Attribute
    {
        public readonly string Type;

        public ContentSerializerAttribute(string type)
        {
            this.Type = type;
        }
    }
}
