using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Package.Content
{
    public abstract class ContentSerializer
    {
        public abstract void Read(Stream stream);
        public abstract void Write(Stream stream);
        public abstract void Write(FileStream fileStream);
        public abstract string GetFileExtension();
    }
}
