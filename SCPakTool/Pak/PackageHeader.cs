using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saturn.PakTool.Pak
{
    public struct PackageHeader
    {
        public long AssetDataTableOffset;
        public int AssetCount;
        public long AssetMetaTableOffset;
    }
}
