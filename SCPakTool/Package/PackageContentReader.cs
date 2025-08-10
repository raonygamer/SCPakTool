using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Pak
{
    public static class PackageContentReader
    {
        public static readonly Dictionary<string, Type> ContentSerializers = [];

        static PackageContentReader()
        {
            ContentSerializers.Clear();

        }
    }
}
