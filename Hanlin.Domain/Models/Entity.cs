using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Domain.Models
{
    public interface IEntity
    {

    }

    public abstract class Entity : IEntity
    {
        public string Id { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}", Id);
        }
    }

    public abstract class Entity<T> : IEntity
    {
        public T Id { get; protected set; }

        public override string ToString()
        {
            return string.Format("Id: {0}", Id);
        }
    }
}
