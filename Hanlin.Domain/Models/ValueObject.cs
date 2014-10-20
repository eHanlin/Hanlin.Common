using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Domain.Models
{
    /// <summary>
    /// Original source: https://github.com/VaughnVernon/IDDD_Samples_NET/blob/114da165fd956375fef0a56b3bd013a633c32ef9/iddd_common/Domain.Model/ValueObject.cs
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// When overriden in a derived class, returns all components of a value objects which constitute its identity.
        /// </summary>
        /// <returns>An ordered list of equality components.</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        #region Equality members

        public bool Equals(ValueObject obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;
            if (object.ReferenceEquals(null, obj)) return false;
            if (this.GetType() != obj.GetType()) return false;

            return GetEqualityComponents().SequenceEqual(obj.GetEqualityComponents());
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;
            if (object.ReferenceEquals(null, obj)) return false;
            if (this.GetType() != obj.GetType()) return false;
            var vo = obj as ValueObject;
            return Equals(vo);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(GetEqualityComponents());
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !Equals(left, right);
        }

        #endregion
    }

    /// <summary>
    /// Original source: https://github.com/VaughnVernon/IDDD_Samples_NET/blob/114da165fd956375fef0a56b3bd013a633c32ef9/iddd_common/Domain.Model/HashCodeHelper.cs
    /// </summary>
    internal static class HashCodeHelper
    {
        public static int CombineHashCodes(IEnumerable<object> objs)
        {
            unchecked
            {
                var hash = 17;
                foreach (var obj in objs)
                    hash = hash * 23 + (obj != null ? obj.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
