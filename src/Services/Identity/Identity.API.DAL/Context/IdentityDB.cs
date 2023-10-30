using System.Data;

using Identity.API.Domain.Entities;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.API.DAL.Context;

public class IdentityDB : IdentityDbContext<User, Role, Guid>
{
	private IDbContextTransaction _currentTransaction;

	public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

	/// <summary>
	/// Проверка наличия активной транзакции
	/// </summary>
	public bool HasActiveTransaction => _currentTransaction != null;

	public IdentityDB(DbContextOptions<IdentityDB> options)
		: base(options)
	{

	}

	/// <summary>
	/// Начать транзакцию, если она ещё не начата
	/// </summary>
	public async Task<IDbContextTransaction> BeginTransactionAsync()
	{
		if (_currentTransaction != null) return null!;

		_currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

		return _currentTransaction;
	}

	/// <summary>
	/// Зафиксировать данные, изменённые в рамках транзакции
	/// </summary>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="InvalidOperationException"></exception>
	public async Task CommitTransactionAsync(IDbContextTransaction transaction)
	{
		if (transaction == null)
			throw new ArgumentNullException(nameof(transaction));

		if (transaction != _currentTransaction)
			throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

		try
		{
			await SaveChangesAsync();
			await transaction.CommitAsync();
		}
		catch
		{
			RollbackTransaction();
			throw;
		}
		finally
		{
			if (_currentTransaction != null)
			{
				_currentTransaction.Dispose();
				_currentTransaction = null!;
			}
		}
	}

	/// <summary>
	/// Откат изменений до точки восстановления, по умолчанию - точка начала транзакции, но могут быть вручную созданные точки с помощью 
	/// transaction.CreateSavepoint()
	/// </summary>
	public void RollbackTransaction()
	{
		try
		{
			_currentTransaction?.Rollback();
		}
		finally
		{
			if (_currentTransaction != null)
			{
				_currentTransaction.Dispose();
				_currentTransaction = null;
			}
		}
	}
}
