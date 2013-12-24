using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanlin.Common.Enums;

namespace Hanlin.Common.Models
{
    public class TextVersion : Enumeration<string>
    {
        public static readonly TextVersion 翰林 = new TextVersion("H", "翰林");
        public static readonly TextVersion 康軒 = new TextVersion("K", "康軒");
        public static readonly TextVersion 南一 = new TextVersion("N", "南一");
        public static readonly TextVersion 部編 = new TextVersion("E", "部編");
        public static readonly TextVersion 通用 = new TextVersion("G", "通用");

        public TextVersion()
        {
        }

        public TextVersion(string value, string name)
            : base(value, name)
        {
        }
    }
}
