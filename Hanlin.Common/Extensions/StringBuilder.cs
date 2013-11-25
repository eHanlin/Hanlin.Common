using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanlin.Common.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void AppendLineFormat(this StringBuilder builder, string format, params object[] args)
        {
            builder.AppendFormat(format, args);
            builder.AppendLine();
        }
    }
}
