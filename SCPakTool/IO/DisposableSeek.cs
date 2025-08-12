using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saturn.PakTool.IO
{
    public class DisposableSeek : IDisposable
    {
        private Stream _stream;
        private long _lastPosition;

        public DisposableSeek(BinaryWriter writer, long position, SeekOrigin origin = SeekOrigin.Begin) :
           this(writer.BaseStream, position, origin)
        { }

        public DisposableSeek(BinaryReader reader, long position, SeekOrigin origin = SeekOrigin.Begin) :
            this(reader.BaseStream, position, origin)
        { }

        public DisposableSeek(Stream stream, long position, SeekOrigin origin = SeekOrigin.Begin)
        {
            _stream = stream;
            _lastPosition = stream.Position;
            _stream.Seek(position, origin);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _stream.Seek(_lastPosition, SeekOrigin.Begin);
        }
    }
}
