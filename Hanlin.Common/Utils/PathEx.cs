using System;
using System.IO;

namespace Hanlin.Common.Utils
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
        
        public static string AppendToFilename(string path, string suffix)
        {
            var dotIndex = path.LastIndexOf(".", System.StringComparison.Ordinal);

            if (dotIndex == -1)
            {
                return path + suffix;
            }
            else
            {
                return path.Substring(0, dotIndex) + suffix + "." + path.Substring(dotIndex + 1);
            }
        }
    }

    
        
}
