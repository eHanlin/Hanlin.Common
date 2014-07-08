using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly List<string> _lookups = new List<string>();

        public IEnumerable<string> Lookups { get { return _lookups; } }

        private const string MsgTemplate = "Cannot find entity of type {0} by lookup data: '{1}'.";

        public EntityNotFoundException(object id) : base(string.Format(MsgTemplate, typeof (T).Name, id))
        {
            _lookups.Add(id.ToString());
        }

        public EntityNotFoundException(IReadOnlyCollection<string> lookups)
            : base(string.Format(MsgTemplate, typeof (T).Name, string.Join(", ", lookups)))
        {
            _lookups.AddRange(lookups);
        }

        public EntityNotFoundException(object id, Exception e)
            : base(string.Format(MsgTemplate, typeof (T).Name, id), e)
        {
            _lookups.Add(id.ToString());
        }

        public EntityNotFoundException(string message) : base(message) { }
        public EntityNotFoundException(string message, Exception e) : base(message, e) { }
    }
}
