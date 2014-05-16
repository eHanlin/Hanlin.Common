namespace Hanlin.Common.Utils
{
    public class FileDescriptor
    {
        public string Name { get; private set; }
        public string ContentType { get; private set; }

        public FileDescriptor(string name, string contentType)
        {
            Name = name;
            ContentType = contentType;
        }
    }
}