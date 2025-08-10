using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPakTool.Common
{
    public class ScopedCursor : IDisposable
    {
        private Stream _stream;
        private long _lastPosition;

        public ScopedCursor(BinaryWriter writer, long position, SeekOrigin origin = SeekOrigin.Begin) :
           this(writer.BaseStream, position, origin)
        { }

        public ScopedCursor(BinaryReader reader, long position, SeekOrigin origin = SeekOrigin.Begin) :
            this(reader.BaseStream, position, origin)
        { }

        public ScopedCursor(Stream stream, long position, SeekOrigin origin = SeekOrigin.Begin)
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
