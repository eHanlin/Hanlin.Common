namespace Hanlin.Domain.Models
{
    public class NameValue
    {
        protected NameValue()
        {
        }

        protected NameValue(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        protected bool Equals(NameValue other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NameValue)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(NameValue left, NameValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NameValue left, NameValue right)
        {
            return !Equals(left, right);
        }
    }
}