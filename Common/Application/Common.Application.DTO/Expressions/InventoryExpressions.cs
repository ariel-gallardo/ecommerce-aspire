using Common.Application.DTO.ValueObjects;
using Common.Domain.ValueObjects;
using System;
using System.Linq.Expressions;

namespace Common.Application.DTO.Expressions
{
    public static class InventoryExpressions
    {/*
        public static Expression<Func<T, bool>> BuildQuantityExpression<T>(
            string operatorQuantity,
            QuantityDTO qtyDto,
            Expression<Func<T, Quantity>> ex)
        {
            if (string.IsNullOrWhiteSpace(operatorQuantity) && qtyDto == null)
                return x => false;

            var qtyDomain = qtyDto.AsDomain();

            var param = ex.Parameters[0];

            var left = ex.Body;

            var right = Expression.Constant(qtyDomain, typeof(Quantity));

            Expression body = operatorQuantity switch
            {
                ">=" => Expression.GreaterThanOrEqual(left, right),
                "<=" => Expression.LessThanOrEqual(left, right),
                ">" => Expression.GreaterThan(left, right),
                "<" => Expression.LessThan(left, right),
                "=" => Expression.Equal(left, right),
                "!=" => Expression.NotEqual(left, right),
                _ => Expression.Constant(false)
            };
            return Expression.Lambda<Func<T, bool>>(body, param);
        }*/
    }
}
