using System;

namespace Hanlin.Domain.Models
{
    // Adapted from: https://github.com/VaughnVernon/IDDD_Samples_NET/blob/master/iddd_common/Domain.Model/Identity.cs
    public abstract class Identity
    {
        public string Id { get; private set; }

        protected Identity()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        protected Identity(string id)
        {
            if (id == null) throw new ArgumentNullException("id");

            Id = id;
        }

        protected bool Equals(Identity other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Identity) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Identity left, Identity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Identity left, Identity right)
        {
            return !Equals(left, right);
        }
    }
}