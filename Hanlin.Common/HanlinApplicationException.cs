using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanlin.Common
{
    public class HanlinApplicationException : Exception
    {
        public HanlinApplicationException(string message, params object[] objs) : base(string.Format(message, objs))
        {
            
        }

        public HanlinApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
