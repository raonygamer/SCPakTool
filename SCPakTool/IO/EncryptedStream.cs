using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saturn.PakTool.IO
{
    public class EncryptedStream : Stream
    {
        public readonly Stream BaseStream;
        public readonly byte[]? Key;
        public readonly long KeyOffset = 0;
        public bool KeepOpen = false;
        public override bool CanRead => BaseStream.CanRead;
        public override bool CanSeek => BaseStream.CanSeek;
        public override bool CanWrite => BaseStream.CanWrite;
        public override long Length => BaseStream.Length;
        public override long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public EncryptedStream(byte[] bytes, byte[]? key = null, long keyOffset = 0, bool keepOpen = false)
        {
            BaseStream = new MemoryStream(bytes);
            Key = key;
            KeyOffset = keyOffset;
            KeepOpen = keepOpen;
        }

        public EncryptedStream(Stream stream, byte[]? key = null, long keyOffset = 0, bool keepOpen = true)
        {
            BaseStream = stream;
            Key = key;
            KeyOffset = keyOffset;
            KeepOpen = keepOpen;
        }

        public override void Flush() => BaseStream.Flush();
        public override long Seek(long offset, SeekOrigin origin = SeekOrigin.Begin) => BaseStream.Seek(offset, origin);
        public override void SetLength(long value) => BaseStream.SetLength(value);

        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = BaseStream.Read(buffer, offset, count);
            if (Key is null)
                return read;
            for (var i = 0; i < read; i++)
            {
                long absPos = Position - read + i;
                if (absPos >= KeyOffset)
                    buffer[offset + i] ^= Key[absPos % Key.Length];
            }
            return read;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (Key is not null)
            {
                for (var i = 0; i < count; i++)
                {
                    long absPos = Position + i;
                    if (absPos >= KeyOffset)
                        buffer[offset + i] ^= Key[absPos % Key.Length];
                }
            }
            BaseStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!KeepOpen)
                    BaseStream.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
