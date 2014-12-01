using System;
using System.Linq.Expressions;

namespace Hanlin.Common.EntityFramework.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> GetPredicate();
    }
}
