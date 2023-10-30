using System.Linq.Expressions;

using Service.Common.DTO;
using Service.Common.Entities.Base.Interfaces;

namespace Service.Common.Extensions;

public static class QueryableExtension
{
	public static IQueryable<T> OrderByColumn<T>(this IQueryable<T> query, OrderDto order)
		where T : IEntity
	{
		ArgumentNullException.ThrowIfNull(query, nameof(query));

		if (order is null)
			return query;

		LambdaExpression CreateExpression(Type type, string propertyName)
		{
			var param = Expression.Parameter(type);

			Expression body = param;
			body = Expression.PropertyOrField(body, propertyName);

			return Expression.Lambda(body, param);
		}

		var type = typeof(T);

		var property = type.GetProperties().FirstOrDefault(n => n.Name.Equals(order.Column, StringComparison.OrdinalIgnoreCase));

		var selector = CreateExpression(type, order.Column);

		var action = order.Direction.Equals("asc", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";

		var enumarableType = typeof(Queryable);
		var method = enumarableType.GetMethods()
			.Where(m => m.Name == action && m.IsGenericMethodDefinition)
			.Where(m =>
			{
				var parameters = m.GetParameters().ToList();

				return parameters.Count == 2;
			}).Single();

		var genericMethod = method
			.MakeGenericMethod(type, property.PropertyType);

		return (IOrderedQueryable<T>)genericMethod
				.Invoke(genericMethod, new object[] { query, selector })!;
	}
}