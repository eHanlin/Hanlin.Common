using System.Collections.Generic;

namespace Hanlin.Domain.Models
{
    public class GenericValueObject<T> : ValueObject
    {
        public GenericValueObject(T id)
        {
            Value = id;
        }

        protected GenericValueObject()
        {
            
        }

        public T Value { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }  
    }
}