using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using PersonalCabinet.API.Domain.Person;

using Service.Common.Entities.App;

namespace PersonalCabinet.API.DAL.Context;

public class PersonalCabinetDB : DbContext
{
	private IDbContextTransaction _currentTransaction;

	public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

	/// <summary>
	/// Проверка наличия активной транзакции
	/// </summary>
	public bool HasActiveTransaction => _currentTransaction != null;

	public DbSet<Source> Sources { get; set; }
	public DbSet<UserInfo> UsersInfo { get; set; }

	public DbSet<UserPerson> UserPersons { get; set; }
	public DbSet<Card> Cards { get; set; }
	public DbSet<LoadedDataType> LoadedDataTypes { get; set; }
	public DbSet<PersonPhoto> Photos { get; set; }
	public DbSet<PersonDocument> Documents { get; set; }

	public PersonalCabinetDB()
	{

	}

	public PersonalCabinetDB(DbContextOptions<PersonalCabinetDB> options)
		: base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		Map(modelBuilder);
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
		if (transaction == null) throw new ArgumentNullException(nameof(transaction));
		if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

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

	private static void Map(ModelBuilder builder)
	{
		builder.ApplyConfiguration(new Source.Map());
		builder.ApplyConfiguration(new UserInfo.Map());

		builder.ApplyConfiguration(new UserPerson.Map());
		builder.ApplyConfiguration(new Card.Map());
		builder.ApplyConfiguration(new LoadedDataType.Map());
		builder.ApplyConfiguration(new PersonPhoto.Map());
		builder.ApplyConfiguration(new PersonDocument.Map());
	}
}
