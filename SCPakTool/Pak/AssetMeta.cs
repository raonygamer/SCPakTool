using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saturn.PakTool.Pak
{
    public class AssetMeta(string name, string type)
    {
        public string Name { get; set; } = name;
        public string Type { get; set; } = type;
        public long Offset { get; set; } = 0L;
        public long Size { get; set; } = 0L;
    }
}
