using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Service.Common.Entities.App;

using StockControl.API.Domain.Stock;

namespace StockControl.API.DAL.Context;

public class StockControlDB : DbContext
{
	private IDbContextTransaction _currentTransaction;

	public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

	/// <summary>
	/// Проверка наличия активной транзакции
	/// </summary>
	public bool HasActiveTransaction => _currentTransaction != null;

	public DbSet<Source> Sources { get; set; }
	public DbSet<UserInfo> UsersInfo { get; set; }

	public DbSet<Classifier> Classifiers { get; set; }
	public DbSet<Organization> Organizations { get; set; }
	public DbSet<Nomenclature> Nomenclatures { get; set; }
	public DbSet<Warehouse> Warehouses { get; set; }

	public DbSet<Receipt> Receipts { get; set; }
	public DbSet<Moving> Movings { get; set; }
	public DbSet<WriteOff> WriteOffs { get; set; }
	public DbSet<ProductFlowType> ProductFlowTypes { get; set; }
	public DbSet<Party> Parties { get; set; }

	public DbSet<StockAvailability> StockAvailabilities { get; set; }

	public StockControlDB()
	{

	}

	public StockControlDB(DbContextOptions<StockControlDB> options)
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

		builder.ApplyConfiguration(new Classifier.Map());
		builder.ApplyConfiguration(new Organization.Map());
		builder.ApplyConfiguration(new Nomenclature.Map());
		builder.ApplyConfiguration(new Warehouse.Map());

		builder.ApplyConfiguration(new Receipt.Map());
		builder.ApplyConfiguration(new Moving.Map());
		builder.ApplyConfiguration(new WriteOff.Map());
		builder.ApplyConfiguration(new ProductFlowType.Map());
		builder.ApplyConfiguration(new Party.Map());

		builder.ApplyConfiguration(new StockAvailability.Map());
	}
}
