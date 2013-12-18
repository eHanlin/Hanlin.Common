using System;
using System.Linq.Expressions;

namespace Hanlin.Common.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> GetPredicate();
    }
}
