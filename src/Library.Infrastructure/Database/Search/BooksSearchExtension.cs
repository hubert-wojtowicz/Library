using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Library.Infrastructure.Database.Search;

public static class BooksSearchExtension
{
    public static IQueryable<T> WhereBooks<T>(this IQueryable<T> books, BooksSearchFilter filter)
    {
        if (books == null)
            throw new ArgumentNullException(nameof(T));

        var predicate = BuildPredicate<T>(filter);
        return books.Where(predicate);
    }

    private static Expression<Func<T, bool>> BuildPredicate<T>(BooksSearchFilter filter)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression? body = VisitFilter(filter, parameter);
        return Expression.Lambda<Func<T, bool>>(body ?? Expression.Constant(true), parameter);
    }

    private static Expression? VisitFilter(BooksSearchFilter filter, ParameterExpression parameter)
    {
        if (filter == null)
        {
            return null;
        }

        Expression leftExpression = VisitFilter(filter.Left, parameter);
        Expression rightExpression = VisitFilter(filter.Right, parameter);

        if (filter.Condition != null)
        {
            MemberExpression propertyExpression = BuildPropertyExpression(parameter, filter.Condition.PropertyName);
            ConstantExpression valueExpression = Expression.Constant(filter.Condition.Value);

            Expression conditionExpression;
            switch (filter.Operator)
            {
                case "EQUAL":
                    conditionExpression = Expression.Equal(propertyExpression, valueExpression);
                    break;
                case "CONTAINS":
                    MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    conditionExpression = Expression.Call(propertyExpression, containsMethod, valueExpression);
                    break;
                default:
                    throw new NotSupportedException($"Operator '{filter.Operator}' is not supported.");
            }

            return conditionExpression;
        }

        if (leftExpression != null && rightExpression != null)
        {
            switch (filter.Operator)
            {
                case "AND":
                    return Expression.AndAlso(leftExpression, rightExpression);
                case "OR":
                    return Expression.OrElse(leftExpression, rightExpression);
                default:
                    throw new NotSupportedException($"Operator '{filter.Operator}' is not supported.");
            }
        }

        return leftExpression ?? rightExpression;
    }

    private static MemberExpression BuildPropertyExpression(Expression expression, string navigationString)
    {
        MemberExpression memberExpression = null;

        foreach (var propertyName in navigationString.Split('.'))
        {
            var propertyInfo = expression.Type.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Invalid property name: {propertyName}");
            }

            if (!propertyInfo.IsDefined(typeof(CanFilterWithAttribute), false))
                throw new ArgumentException($"Navigation string {navigationString} is incorrect.");

            memberExpression = Expression.Property(expression, propertyInfo);

            if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                // Handle collection indexing
                memberExpression = Expression.Property(memberExpression, "FirstOrDefault");
            }

            expression = memberExpression;
        }

        return memberExpression;
    }
}