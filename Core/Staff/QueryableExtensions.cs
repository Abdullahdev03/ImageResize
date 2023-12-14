using Core.ViewModel.PagingAndFiltration;
using Core.ViewModel;
using System.Linq.Expressions;
using System.Reflection;
using Core.Enums;

namespace Core.Staff;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, SortRule rule)
    {
        if (string.IsNullOrEmpty(rule?.SortProperty)) return query;

        Type type = typeof(T);
        ParameterExpression pe = Expression.Parameter(type, "obj");

        PropertyInfo propInfo = type.GetProperty(rule.SortProperty);
        if (propInfo == null)
        {
            var fixedPropertyName = rule.SortProperty[0].ToString().ToUpper() +
                             rule.SortProperty.Substring(1, rule.SortProperty.Length - 1);
            propInfo = type.GetProperty(fixedPropertyName);
            if (propInfo == null) throw new ArgumentException($"Invalid property name '{rule.SortProperty}' for type '{type.FullName}'.");

        }


        var expr = Expression.MakeMemberAccess(pe, propInfo);
        var orderByExpression = Expression.Lambda(expr, pe);

        MethodCallExpression orderByCallExpression = Expression.Call(typeof(Queryable), rule.SortDir == SortDirection.Ascending ? "OrderBy" : "OrderByDescending",
            new Type[] { type, propInfo.PropertyType }, query.Expression, orderByExpression);

        return query.Provider.CreateQuery<T>(orderByCallExpression);
    }

    public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, SortRule rule, SortDirection defaultSortDirection = SortDirection.Ascending)
    {
        if (string.IsNullOrEmpty(rule?.SortProperty))
        {
            return query;
        }

        return query.OrderBy(rule);
    }




    public static IQueryable<T> Where<T>(this IQueryable<T> query, FilterDto filterDto)
    {
        if (string.IsNullOrWhiteSpace(filterDto.Value) || string.IsNullOrWhiteSpace(filterDto.Property))
        {
            return query;
        }
        Type type = typeof(T);
        var propertyInfo = type.GetProperty(filterDto.Property);
        if (propertyInfo == null)
        {
            var fixedPropertyName = filterDto.Property[0].ToString().ToUpper() +
                             filterDto.Property.Substring(1, filterDto.Property.Length - 1);
            propertyInfo = type.GetProperty(fixedPropertyName);
            if (propertyInfo == null) throw new ArgumentException($"Invalid property name '{filterDto.Property}' for type '{type.FullName}'.");
        }

        var parameter = Expression.Parameter(type, "x");
        var property = Expression.Property(parameter, filterDto.Property);
        var value = Expression.Constant(filterDto.Value.ToLower());
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

        var toLowerProperty = Expression.Call(property, toLowerMethod);
        var toLowerValue = Expression.Call(value, toLowerMethod);

        var containsExpression = Expression.Call(toLowerProperty, containsMethod, toLowerValue);

        var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
        return query.Where(lambda);

    }

}
