using System.Collections.Generic;

namespace Hanlin.Domain.Models
{
    public class GenericValueObject<T> : ValueObject
    {
        public GenericValueObject(T id)
        {
            Value = id;
        }

        public T Value { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}