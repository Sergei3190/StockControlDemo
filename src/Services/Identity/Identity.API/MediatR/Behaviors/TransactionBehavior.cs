using EventBus.Extensions;

using Identity.API.DAL.Context;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Identity.API.MediatR.Behaviors;

/// <summary>
/// Обвертывание транзакциями обраотчиков команд и запросов MediatR
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
	private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
	private readonly IdentityDB _db;

	public TransactionBehavior(IdentityDB db,
		ILogger<TransactionBehavior<TRequest, TResponse>> logger)
	{
		_db = db ?? throw new ArgumentException(nameof(db));
		_logger = logger ?? throw new ArgumentException(nameof(ILogger));
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var response = default(TResponse);
		var typeName = request.GetType().GetGenericTypeName(); // получаем наименование исполняемой команды MediatR

		try
		{
			if (_db.HasActiveTransaction)
			{
				return await next();
			}

			var strategy = _db.Database.CreateExecutionStrategy();

			await strategy.ExecuteAsync(async () =>
			{
				Guid transactionId;

				await using var transaction = await _db.BeginTransactionAsync();
				using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
				{
					_logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

					response = await next();

					_logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

					await _db.CommitTransactionAsync(transaction);

					transactionId = transaction.TransactionId;
				}
			});

			return response;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeName, request);

			throw;
		}
	}
}
