using SCPakTool.Common;
using SCPakTool.Pak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Package.Content
{
    public abstract class ContentSerializer
    {
        public readonly ContentDescriptor ContentDescriptor;

        protected ContentSerializer(ContentDescriptor descriptor)
        {
            ContentDescriptor = descriptor;
        }

        public abstract void ReadFromPackage(PackageFile package);

        public abstract void WriteToPackage(PackageFile package);

        public virtual void WriteToDirectory(DirectoryInfo directory, IEnumerable<string> extensions)
        {
            if (!directory.Exists)
                directory.Create();

            var relativeDir = Path.GetDirectoryName(ContentDescriptor.Name);
            if (!string.IsNullOrEmpty(relativeDir))
            {
                var fullDirPath = Path.Combine(directory.FullName, relativeDir);
                Directory.CreateDirectory(fullDirPath);
            }
        }

        public abstract IEnumerable<string> GetFileExtensions();
    }
}
