using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanlin.Common.Extensions;

namespace Hanlin.Common
{
    public class EntityNotFoundException : HanlinApplicationException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class EntityNotFoundException<T> : EntityNotFoundException
    {
        private const string MsgTemplate = "Cannot find entity of type {0} by lookup data: '{1}'.";

        public EntityNotFoundException(object id) : base(string.Format(MsgTemplate, typeof(T).Name, id)) { }
        public EntityNotFoundException(IEnumerable<object> ids) : base(string.Format(MsgTemplate, typeof(T).Name, ids.AsString())) { }
        public EntityNotFoundException(object id, Exception e) : base(string.Format(MsgTemplate, typeof(T).Name, id), e) { }
        public EntityNotFoundException(string message) : base(message) { }
        public EntityNotFoundException(string message, Exception e) : base(message, e) { }
    }
}
