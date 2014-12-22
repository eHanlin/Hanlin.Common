using System;
using System.Globalization;

namespace Hanlin.Domain.Models
{
    public abstract class SurrogateIdentity : IEntity, IEquatable<SurrogateIdentity>
    {
        public long Id { get; protected set; }

        public bool IsIdAssigned { get { return Id > 0; } }
        public string IdStr { get { return Id.ToString(CultureInfo.InvariantCulture); } }

        protected SurrogateIdentity()
        {
            
        }

        protected SurrogateIdentity(long id)
        {
            Id = id;
        }

        protected SurrogateIdentity(string id)
        {
            long parsed;
            if (long.TryParse(id, out parsed))
            {
                Id = parsed;
            }
            else
            {
                throw new ArgumentException(string.Format("Value {0} is not a valid surrogate identity value", id));
            }
        }

        public override string ToString()
        {
            return Id.ToString(CultureInfo.InvariantCulture);
        }

        public bool Equals(SurrogateIdentity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SurrogateIdentity) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}