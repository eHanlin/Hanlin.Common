using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LinqKit;

namespace Hanlin.Common.EntityFramework.Specifications
{
    public class FalseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> GetPredicate()
        {
            return PredicateBuilder.False<T>();
        }
    }

    public class TrueSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> GetPredicate()
        {
            return PredicateBuilder.True<T>();
        }
    }

    public class NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _operand;

        public NotSpecification(ISpecification<T> operand)
        {
            _operand = operand;
        }

        public Expression<Func<T, bool>> GetPredicate()
        {
            var operandExpr = _operand.GetPredicate();
            var innerInvocation = Expression.Invoke((Expression) operandExpr, (IEnumerable<Expression>) operandExpr.Parameters);

            var notExpr = Expression.Lambda<Func<T, bool>>(Expression.Not(innerInvocation), operandExpr.Parameters);
            return notExpr;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _lhs;
        private readonly ISpecification<T> _rhs;

        public AndSpecification(ISpecification<T> lhs, ISpecification<T> rhs)
        {
            this._lhs = lhs;
            this._rhs = rhs;
        }

        public Expression<Func<T, bool>> GetPredicate()
        {
            return _lhs.GetPredicate().And(_rhs.GetPredicate());
        }
    }

    public class OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _lhs;
        private readonly ISpecification<T> _rhs;

        public OrSpecification(ISpecification<T> lhs, ISpecification<T> rhs)
        {
            this._lhs = lhs;
            this._rhs = rhs;
        }

        public Expression<Func<T, bool>> GetPredicate()
        {
            return _lhs.GetPredicate().Or(_rhs.GetPredicate());
        }
    }
}
