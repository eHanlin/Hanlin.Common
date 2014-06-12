using System;
using System.Drawing;
using System.IO;

namespace Hanlin.Common.Utils
{
    public class ContentMemoryStream : MemoryStream
    {
        public static ContentMemoryStream CopyFrom(Stream stream, string contentType, string name = null)
        {
            if (stream.Length > int.MaxValue)
            {
                throw new ArgumentException("Source stream is too large with length: " + stream.Length);
            }

            var contentStream = new ContentMemoryStream(name, contentType, (int)stream.Length);
            stream.CopyTo(contentStream);
            contentStream.Position = 0;
            return contentStream;
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
            var copy = CopyFrom(this, this.ContentType, this.Name);
            return copy;
        }

        public override string ToString()
        {
            return string.Format("Content stream: {0}, legnth {1}", ContentType, Length);
        }
    }
}