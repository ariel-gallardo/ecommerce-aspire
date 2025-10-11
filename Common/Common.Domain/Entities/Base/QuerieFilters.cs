using Common.Domain.Entities.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Domain.Entities.Base
{
    public abstract class QuerieFilters<T> : IQuerieFilters<T> where T : class, IEntity
    {
        public virtual Expression<Func<T, bool>> Expressions
        {
            get
            {
                var expressions = GetExpressions();

                if (!expressions.Any())
                    return x => true;

                var parameter = Expression.Parameter(typeof(T), "x");
                Expression? combined = null;

                foreach (var expr in expressions)
                {
                    var replaced = new ParameterReplacer(parameter).Visit(expr.Body);
                    combined = combined == null
                        ? replaced
                        : Expression.OrElse(combined, replaced);
                }

                return Expression.Lambda<Func<T, bool>>(combined!, parameter);
            }
        }

        protected virtual IEnumerable<Expression<Func<T, bool>>> GetExpressions()
        {
            var props = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => typeof(Expression<Func<T, bool>>).IsAssignableFrom(p.PropertyType));

            foreach (var prop in props)
            {
                var value = prop.GetValue(this) as Expression<Func<T, bool>>;
                if (value != null)
                    yield return value;
            }

            var fields = GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => typeof(Expression<Func<T, bool>>).IsAssignableFrom(f.FieldType));

            foreach (var field in fields)
            {
                var value = field.GetValue(this) as Expression<Func<T, bool>>;
                if (value != null)
                    yield return value;
            }
        }

        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;
            public ParameterReplacer(ParameterExpression parameter) => _parameter = parameter;
            protected override Expression VisitParameter(ParameterExpression node) => _parameter;
        }
    }
}
