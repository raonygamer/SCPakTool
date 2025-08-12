using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saturn.PakTool.Pak
{
    public readonly struct Asset(AssetMeta meta, AssetData data)
    {
        public readonly AssetMeta Meta = meta;
        public readonly AssetData Data = data;
    }
}
