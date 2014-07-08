using System;
using System.Drawing;
using System.IO;

namespace Hanlin.Common.Utils
{
    public sealed class ContentMemoryStream : MemoryStream
    {
        public ContentMemoryStream(Stream stream, string contentType, string name = null) : this(name, contentType, (int)stream.Length)
        {
            if (stream.Length > int.MaxValue)
            {
                throw new ArgumentException("Source stream is too large with length: " + stream.Length);
            }

            stream.CopyTo(this);
            Position = 0;
        }

        public ContentMemoryStream(string contentType, int capacity = 0)
            : this(null, contentType, capacity)
        {
            ContentType = contentType;
        }

        public ContentMemoryStream(string name, string contentType, int capacity = 0)
            : base(capacity)
        {
            ContentType = contentType;
            Name = name ?? string.Empty;
        }

        public string Name { get; private set; }

        public string ContentType { get; private set; }

        public Size EmbedSize { get; set; }

        public ContentMemoryStream Copy()
        {
            var copy = new ContentMemoryStream(this, this.ContentType, this.Name)
            {
                EmbedSize = this.EmbedSize
            };
            return copy;
        }

        public override string ToString()
        {
            return string.Format("Content stream: {0}, legnth {1}", ContentType, Length);
        }
    }
}