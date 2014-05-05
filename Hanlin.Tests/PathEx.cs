using System;
using System.IO;

namespace Hanlin.Tests
{
    public static class PathEx
    {
        public static string AppendToPath(string path, string appendStr)
        {
            string output = null;
            var extIndex = path.LastIndexOf('.');

            if (extIndex == 0)
            {
                throw new ArgumentException("Invalid filename: " + path);
            }

            if (extIndex != -1)
            {
                output = path.Insert(extIndex, appendStr);
            }
            return output;
        }
        
        public static string AppendToFilename(string filename, string suffix)
        {
            return Path.GetFileNameWithoutExtension(filename) + suffix + Path.GetExtension(filename);
        }
    }

    
        
}
