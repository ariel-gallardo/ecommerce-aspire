using Common.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Infrastructure
{
    public class ExpressionBuilder : IExpressionBuilder, IScoped
    {
        public Expression<Func<T, bool>> Build<T>(object filter) where T : class
        {
            var expressions = new List<Expression<Func<T, bool>>>();
            var type = filter.GetType();

            // Campos
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (typeof(Expression<Func<T, bool>>).IsAssignableFrom(field.FieldType))
                {
                    var value = field.GetValue(filter) as Expression<Func<T, bool>>;
                    if (value != null)
                        expressions.Add(value);
                }
            }

            // Propiedades
            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (typeof(Expression<Func<T, bool>>).IsAssignableFrom(prop.PropertyType))
                {
                    var value = prop.GetValue(filter) as Expression<Func<T, bool>>;
                    if (value != null)
                        expressions.Add(value);
                }
            }

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

        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;
            public ParameterReplacer(ParameterExpression parameter) => _parameter = parameter;
            protected override Expression VisitParameter(ParameterExpression node) => _parameter;
        }
    }
}
