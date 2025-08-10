using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Common
{
    public class EncryptedStream : Stream
    {
        public Stream BaseStream => _baseStream;

        private Stream _baseStream;
        private byte[]? _key;
        private long _keyOffset;

        public EncryptedStream(Stream stream, byte[]? key = null, long keyOffset = 0)
        {
            _baseStream = stream;
            _key = key;
            _keyOffset = keyOffset;
        }

        public override bool CanRead => _baseStream.CanRead;

        public override bool CanSeek => _baseStream.CanSeek;

        public override bool CanWrite => _baseStream.CanWrite;

        public override long Length => _baseStream.Length;

        public override long Position { get => _baseStream.Position; set => _baseStream.Position = value; }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = _baseStream.Read(buffer, offset, count);
            if (_key is null)
                return read;
            for (var i = 0; i < read; i++)
            {
                long absPos = Position - read + i;
                if (absPos >= _keyOffset)
                    buffer[offset + i] ^= _key[absPos % _key.Length];
            }
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_key is not null)
            {
                for (var i = 0; i < count; i++)
                {
                    long absPos = Position + i;
                    if (absPos >= _keyOffset)
                        buffer[offset + i] ^= _key[absPos % _key.Length];
                }
            }
            _baseStream.Write(buffer, offset, count);
        }
    }
}
