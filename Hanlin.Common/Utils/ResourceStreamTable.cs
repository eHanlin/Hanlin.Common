﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hanlin.Common.Utils
{
    public class ResourceStreamTable : Dictionary<string, ContentMemoryStream>, IDisposable
    {
        public ICollection<string> WriteTo(string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            var outputPaths = new List<string>();

            foreach (var entry in this)
            {
                var path = Path.Combine(outputDir, entry.Key);
                using (var output = new FileStream(path, FileMode.Create))
                {
                    entry.Value.CopyTo(output);
                }
                outputPaths.Add(path);
            }
            return outputPaths;
        }

        /// <summary>
        /// If an entry with the same key already exists in the table, the incoming entry will be ignored.
        /// </summary>
        /// <param name="other">The entries to be merged.</param>
        /// <returns>Returns this instance</returns>
        public ResourceStreamTable Merge(ResourceStreamTable other)
        {
            foreach (var entry in other)
            {
                if (!ContainsKey(entry.Key))
                {
                    this[entry.Key] = entry.Value;
                }
            }
            return this;
        }

        public ResourceStreamTable Copy()
        {
            var copy = new ResourceStreamTable();
            foreach (var entry in this)
            {
                copy[entry.Key] = entry.Value.Copy();
            }
            return copy;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append("ResourceStreamTable: { ");
            foreach (var entry in this)
            {
                builder.Append(string.Format("[{0}: {1}]", entry.Key, entry.Value));
            }
            builder.Append(" }");

            return builder.ToString();
        }

        public void Dispose()
        {
            foreach (var stream in Values)
            {
                stream.Dispose();
            }
        }
    }
}
