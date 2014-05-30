using System;

namespace Hanlin.Common.Utils
{
    public class FileDescriptor
    {
        public string Name { get; private set; }
        public string ContentType { get; private set; }

        public FileDescriptor(string name, string contentType)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name is required.");
            if (string.IsNullOrEmpty(contentType)) throw new ArgumentException("Content type is required.");

            Name = name;
            ContentType = contentType;
        }
    }
}