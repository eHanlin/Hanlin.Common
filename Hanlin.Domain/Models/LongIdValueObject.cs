
namespace Hanlin.Domain.Models
{
    public class LongIdValueObject : GenericValueObject<long>
    {
        public LongIdValueObject(long id)
            : base(id)
        {
        }

        public LongIdValueObject(string idStr)
            : base(long.Parse(idStr))
        {
        }
    }
}
