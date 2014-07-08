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

        private readonly string _path;

        public bool AutoClean { get; set; }

        public Workspace(params string[] pathSegments)
        {
            if (pathSegments == null) throw new ArgumentNullException("pathSegments");
            if (pathSegments.Length == 0) throw new ArgumentException("Path required.");

            _path = System.IO.Path.Combine(pathSegments);
            Directory.CreateDirectory(_path);

            AutoClean = true;
        }

        public void Dispose()
        {
            if (AutoClean)
            {
                try
                {
                    Directory.Delete(_path, true);
                }
                catch(Exception e)
                {
                    Log.Warn(e.Message, e);
                }
            }
        }

        public string Path { get { return _path; } }

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
                return Path;
            }
            
            var destPath = System.IO.Path.Combine(new [] { Path }.Concat(pathSegments).ToArray());
            
            destPath = System.IO.Path.GetFullPath(destPath);

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destPath));

            return destPath;
        }

        public Workspace CreateSubWorkspace(string subWorkspaceName)
        {
            return new Workspace(_path, subWorkspaceName) { AutoClean = AutoClean };
        }
    }
}