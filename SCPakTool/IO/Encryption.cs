using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saturn.PakTool.IO
{
    public static class Encryption
    {
        public static string? GetString(byte[]? bytes)
        {
            return bytes is not null ? Encoding.UTF8.GetString(bytes) : null;
        }

        public static byte[]? GetBytes(string? str)
        {
            return str is not null ? Encoding.UTF8.GetBytes(str) : null;
        }

        public static void XorBytes(byte[] bytes, byte[]? key, int decryptionOffset = 0, bool startKeyOnOffset = false)
        {
            if (key is null)
                return;
            for (var i = decryptionOffset; i < bytes.Length; i++)
            {
                int rawIndex = (i - (startKeyOnOffset ? decryptionOffset : 0)) % key.Length;
                int keyIndex = rawIndex < 0 ? rawIndex + key.Length : rawIndex;
                bytes[i] ^= key[keyIndex];
            }
        }

        public static void XorBytes(Span<byte> bytes, byte[]? key, int decryptionOffset = 0, bool startKeyOnOffset = false)
        {
            if (key is null)
                return;
            for (var i = decryptionOffset; i < bytes.Length; i++)
            {
                int rawIndex = (i - (startKeyOnOffset ? decryptionOffset : 0)) % key.Length;
                int keyIndex = rawIndex < 0 ? rawIndex + key.Length : rawIndex;
                bytes[i] ^= key[keyIndex];
            }
        }

        public static byte[] XorBytes(ReadOnlySpan<byte> bytes, byte[]? key, int decryptionOffset = 0, bool startKeyOnOffset = false)
        {
            byte[] buffer = bytes.ToArray();
            if (key is null)
                return buffer;
            for (var i = decryptionOffset; i < bytes.Length; i++)
            {
                int rawIndex = (i - (startKeyOnOffset ? decryptionOffset : 0)) % key.Length;
                int keyIndex = rawIndex < 0 ? rawIndex + key.Length : rawIndex;
                buffer[i] ^= key[keyIndex];
            }
            return buffer;
        }
    }
}
