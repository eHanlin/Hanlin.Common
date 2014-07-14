using System;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace Hanlin.Common.Utils
{
    public class Workspace : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _basePath;

        public bool AutoClean { get; set; }

        public Workspace(params string[] basePath)
        {
            if (basePath == null) throw new ArgumentNullException("basePath");
            if (basePath.Length == 0) throw new ArgumentException("Path required.");

            _basePath = System.IO.Path.Combine(basePath);
            Directory.CreateDirectory(_basePath);

            AutoClean = true;
        }

        public void Dispose()
        {
            if (AutoClean)
            {
                try
                {
                    Directory.Delete(_basePath, true);
                }
                catch(Exception e)
                {
                    Log.Warn(e.Message, e);
                }
            }
        }

        public string BasePath { get { return _basePath; } }

/*        public void CreatePath(params string[] pathSegments)
        {
            if (pathSegments == null || pathSegments.Length == 0)
            {
                throw new ArgumentException("Path segments required.");
            }

            var destPath = PathTo(pathSegments) + System.IO.Path.DirectorySeparatorChar;
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destPath));
        }*/

        public string PathTo(params string[] pathSegments)
        {
            if (pathSegments == null || pathSegments.Length == 0)
            {
                return BasePath;
            }
            
            var destPath = System.IO.Path.Combine(new [] { BasePath }.Concat(pathSegments).ToArray());
            
            destPath = System.IO.Path.GetFullPath(destPath);

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destPath));

            return destPath;
        }

        public Workspace CreateSubWorkspace(string subWorkspaceName)
        {
            return new Workspace(_basePath, subWorkspaceName) { AutoClean = AutoClean };
        }
    }
}