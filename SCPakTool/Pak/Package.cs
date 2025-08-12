using Saturn.PakTool.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saturn.PakTool.Pak
{
    public class Package : IEnumerable<Asset>
    {
        public readonly static string FileSignature = "PK2\0";
        public readonly PackageEncryption? Encryption;
        private readonly List<Asset> _assets = [];
        public IReadOnlyList<Asset> Assets => _assets;

        public Package(PackageEncryption? encryption = null)
        {
            Encryption = encryption;
        }

        public Package(string path, PackageEncryption? encryption = null) :
            this(encryption)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("Couldn't find .pak file!", fileInfo.FullName);
            using var fileStream = fileInfo.OpenRead();
            if (!IsPakFile(fileStream))
                throw new InvalidDataException($"The file '{fileInfo.FullName}' is not a valid .pak file!");
            using var metadataStream = new EncryptedStream(fileStream, encryption?.Header, 4);
            using var metadataReader = new BinaryReader(metadataStream);
            var header = ReadHeader(metadataReader);
            using var assetStream = new EncryptedStream(fileStream, encryption?.Content, header.AssetDataTableOffset);
            using var assetReader = new BinaryReader(assetStream);
            ReadAllAssets(metadataReader, assetReader, header);
        }

        public Package(byte[] bytes, PackageEncryption? encryption = null) : 
            this(encryption)
        {

        }

        public void ReadAllAssets(BinaryReader metadataReader, BinaryReader assetReader, PackageHeader header)
        {
            using (new DisposableSeek(metadataReader, header.AssetMetaTableOffset))
            {
                for (var i = 0; i < header.AssetCount; i++)
                {
                    var name = metadataReader.ReadString();
                    var type = metadataReader.ReadString();
                    var offset = metadataReader.ReadInt64();
                    var size = metadataReader.ReadInt64();
                    using (new DisposableSeek(assetReader, header.AssetDataTableOffset + offset))
                    {
                        var data = assetReader.ReadBytes((int)size);
                        _assets.Add(new Asset(new AssetMeta(name, type)
                        {
                            Offset = offset,
                            Size = size
                        }, new AssetData(data)));
                    }
                }
            }
        }

        public static PackageHeader ReadHeader(BinaryReader reader)
        {
            using (new DisposableSeek(reader, 4))
            {
                return new PackageHeader
                {
                    AssetDataTableOffset = reader.ReadInt64(),
                    AssetCount = reader.ReadInt32(),
                    AssetMetaTableOffset = reader.BaseStream.Position
                };
            }
        }

        public static bool IsPakFile(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.UTF8, true);
            using (new DisposableSeek(reader, stream.Position))
                return Encoding.UTF8.GetString(reader.ReadBytes(4)) == FileSignature;
        }

        public static bool IsPakFile(byte[] bytes)
        {
            using var ms = new MemoryStream(bytes);
            return IsPakFile(ms);
        }

        public static bool IsPakFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return false;
            }
            using var fs = fileInfo.OpenRead();
            return IsPakFile(fs);
        }

        public IEnumerator<Asset> GetEnumerator() => Assets.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
