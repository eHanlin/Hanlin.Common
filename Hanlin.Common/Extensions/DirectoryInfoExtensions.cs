using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hanlin.Common.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static void Empty(this DirectoryInfo directory)
        {
            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles("*.*"))
                {
                    file.Delete();
                }

                foreach (var dir in directory.GetDirectories("*"))
                {
                    dir.Delete(true);
                }
            }
        }
    }
}
