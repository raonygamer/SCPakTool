using SCPakTool.Common;
using SCPakTool.Package.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(o => o.GetTypes().Where(t =>
            {
                return t.GetCustomAttribute<ContentSerializerAttribute>() is not null;
            }));

            foreach (var type in types)
            {
                ContentSerializerAttribute serializerAttr = type.GetCustomAttribute<ContentSerializerAttribute>()!;
                ContentSerializers.Add(serializerAttr.Type, type);
            }
        }

        public static ContentSerializer? GetNewSerializerFor(ContentDescriptor descriptor)
        {
            if (!ContentSerializers.TryGetValue(descriptor.Type, out var serializerType))
                return null;
            return Activator.CreateInstance(serializerType, true, descriptor) as ContentSerializer;
        }

        public static void WriteContentToFile(FileStream[] fileStreams, ContentSerializer serializer, PackageFile package, ContentDescriptor descriptor)
        {
            //using (new ScopedCursor(package.ContentStream, package.ContentOffset + descriptor.Offset))
            //{
            //    serializer.ReadFromPackage(package.ContentStream);
            //    serializer.Write(fileStreams);
            //}
        }
    }
}
