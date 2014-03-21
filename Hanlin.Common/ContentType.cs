using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Hanlin.Common.Collections;

namespace Hanlin.Common
{
    public static class ContentType
    {
        public const string png = "image/png";
        public const string mp3 = "audio/mpeg";
        public const string bin = "application/octet-stream";
        public const string snappy = "application/gsnappy";

        public const string text_xml = "text/xml";
        public const string application_xml = "application/xml";

        public const string doc = "application/msword";
        public const string docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";


        public const string unknown = "application/unknown";

        private static BiDictionary<string, string> _mapping = new BiDictionary<string, string>();

        static ContentType()
        {
            _mapping.Add("png", png);
            _mapping.Add("mp3", mp3);
            _mapping.Add("bin", bin);
            _mapping.Add("snappy", snappy);
        }

        public static string InferContentType(string filename)
        {
            if (!Path.HasExtension(filename)) return null;

            var ext = Path.GetExtension(filename);

            if (ext != null && ext.StartsWith("."))
            {
                ext = ext.Substring(1);
            }

            switch (ext)
            {
                case "png":
                    return png;
                case "snappy":
                    return snappy;
                case "mp3":
                    return mp3;
                default:
                    return unknown;
            }
        }

        public static string GetContentType(string ext)
        {
            if (ext.StartsWith(".")) ext = ext.Substring(1);

            string contentType;
            if (!_mapping.TryGetByFirst(ext, out contentType))
            {
                contentType = unknown;
            }

            return contentType;
        }

        public static string GetExtension(string contentType)
        {
            string ext;
            if (!_mapping.TryGetBySecond(contentType, out ext))
            {
                ext = "unknown";
            }

            return ext;
        }
    }
}
