using System;
using System.Linq.Expressions;
using LinqKit;

namespace Hanlin.Common.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> GetPredicate();
    }

    public abstract class Specification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> GetPredicateExpanded()
        {
            return GetPredicate().Expand();
        } 

        public abstract Expression<Func<T, bool>> GetPredicate();
    }
}
