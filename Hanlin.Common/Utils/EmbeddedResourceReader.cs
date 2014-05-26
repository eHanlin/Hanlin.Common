using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Common.Utils
{
    public static class EmbeddedResourceReader
    {
        public static string GetEmbeddedResource(string name, string inNamespace, Assembly inAssembly)
        {
            string resource = null;
            var resName = inNamespace + "." + name;

            // See: http://stackoverflow.com/a/3314213/494297
            using (var stream = inAssembly.GetManifestResourceStream(resName))
            {
                if (stream == null)
                {
                    throw new ArgumentException("Cannot open embedded resource with name: " + resName);
                }

                using (var reader = new StreamReader(stream))
                {
                    resource = reader.ReadToEnd();
                }
            }

            return resource;
        }
    }
}
