using Microsoft.EntityFrameworkCore;

namespace IntegrationEventLogEF;

// данный класс применяем если не используем MediatR, Behavior которого реагирует на запросы, в которых фигурируют команды, объявленные в MediatR

/// <summary>
/// Класс устойчивой транзакции
/// </summary>
public class ResilientTransaction
{
    private readonly DbContext _context;
    private ResilientTransaction(DbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// Метода создания нового экземпляра класса ResilientTransaction с локальным контекстом подписчика/микросервиса событий
    /// </summary>
    public static ResilientTransaction New(DbContext context) => new(context);

    /// <summary>
    /// Выполняем задачу атомарно, те либо выполнятся все действия задачи, либо не выполнится ни одно, 
    /// с использованием локального контекстом подписчика/микросервиса событий
    /// </summary>
    public async Task ExecuteAsync(Func<Task> action)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }
}
