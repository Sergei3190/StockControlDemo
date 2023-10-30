using IntegrationEventLogEF;

using Microsoft.EntityFrameworkCore;

using Service.Common.DTO;
using Service.Common.Entities.App;
using Service.Common.Entities.Base;
using Service.Common.Interfaces;

using StockControl.API.DAL.Context;
using StockControl.API.Domain;
using StockControl.API.Domain.Stock;

namespace StockControl.API.Services;

public class DbInitializerService : IDbInitializerService
{
	private const string DbName = "Stock Control";

	private readonly StockControlDB _db;
	private readonly IntegrationEventLogContext _eventLogContext;
	private readonly ILogger<DbInitializerService> _logger;

	public DbInitializerService(
		StockControlDB db,
		IntegrationEventLogContext eventLogContext,
		ILogger<DbInitializerService> logger)
	{
		_db = db;
		_eventLogContext = eventLogContext;
		_logger = logger;
	}

	public async Task InitializeAsync(DbInitializerDto dto, CancellationToken cancel = default)
	{
		try
		{
			_logger.LogInformation("Инициализация {0} БД...", DbName);

			if (dto.Recreate)
				await RemoveAsync(cancel).ConfigureAwait(false);

			_logger.LogInformation("Применение миграций {0} БД...", DbName);
			await _db.Database.MigrateAsync(cancel).ConfigureAwait(false);
			await _eventLogContext.Database.MigrateAsync(cancel).ConfigureAwait(false);
			_logger.LogInformation("Применение миграций {0} БД выполнено", DbName);

			if (!dto.IsFunctionalTest)
				await InitializerStaticDataAsync(cancel).ConfigureAwait(false);

			if (dto.AddTestData)
			{
				var strategy = _db.Database.CreateExecutionStrategy();

				await strategy.ExecuteAsync(async () =>
				{
					await using var transaction = await _db.Database.BeginTransactionAsync(cancel).ConfigureAwait(false);

					_logger.LogInformation("Инициализация БД тестовыми данными...");
					await InitializerUsersAsync(cancel).ConfigureAwait(false);
					await InitializerOrganizationsAsync(cancel).ConfigureAwait(false);
					await InitializerNomenclaturesAsync(cancel).ConfigureAwait(false);
					await InitializerWarehousesAsync(cancel).ConfigureAwait(false);
					await InitializerPartiesAsync(cancel).ConfigureAwait(false);
					await InitializerReceiptsAsync(cancel).ConfigureAwait(false);
					await InitializerStockAvailabilitiesAsync(cancel).ConfigureAwait(false);

					await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
					await transaction.CommitAsync().ConfigureAwait(false);
					_logger.LogInformation("Инициализация БД тестовыми данными выполнена успешно");
				})
					.ConfigureAwait(false);
			}

			_logger.LogInformation("Инициализация {0} БД выполнена успешно", DbName);
		}
		catch (Exception ex)
		{
			_logger.LogError("Произошла ошибка при инициализации {0} БД: {1}", DbName, ex);
			throw;
		}
	}

	public async Task<bool> RemoveAsync(CancellationToken cancel = default)
	{
		_logger.LogInformation("Удаление {0} БД...", DbName);

		var result = await _db.Database.EnsureDeletedAsync(cancel).ConfigureAwait(false);

		_logger.LogInformation("{0}", result ? string.Format("Удаление {0} БД выполнено успешно", DbName)
			: string.Format("Удаление {0} БД не выполнено", DbName));

		return result;
	}

	private async Task InitializerStaticDataAsync(CancellationToken cancel = default)
	{
		_logger.LogInformation("Инициализация статических данных {0} БД...", DbName);

		// чтобы выполнить в с отказоустойчивостью в рамках транзакции
		var strategy = _db.Database.CreateExecutionStrategy();

		await strategy.ExecuteAsync(async () =>
		{
			await using var transaction = await _db.Database.BeginTransactionAsync(cancel).ConfigureAwait(false);

			await AddOrUpdateStaticDataAsync().ConfigureAwait(false);

			await _db.SaveChangesAsync().ConfigureAwait(false);
			await transaction.CommitAsync().ConfigureAwait(false);
		})
			.ConfigureAwait(false);

		_logger.LogInformation("Инициализация статических данных {0} БД выполнена успешно", DbName);
	}

	private async Task AddOrUpdateStaticDataAsync()
	{
		async Task RunAddOrUpdateAsync<TEntity>(params TEntity[] entities) where TEntity : DictionaryEntity
		{
			if (await _db.Set<TEntity>().AnyAsync().ConfigureAwait(false))
			{
				var sourcesDB = await _db.Set<TEntity>().ToArrayAsync().ConfigureAwait(false);

				// обновляем активность и добавляем новые элементы
				foreach (var item in entities)
				{
					var sourceDB = sourcesDB.FirstOrDefault(c => c.Id == item.Id);

					if (sourceDB is null)
						await _db.AddAsync(item).ConfigureAwait(false);
					else
					{
						sourceDB.Name = item.Name;
						sourceDB.Mnemo = item.Mnemo;
						sourceDB.IsActive = item.IsActive;
					}
				}
			}
			else
				await _db.AddRangeAsync(entities).ConfigureAwait(false);
		}

		await RunAddOrUpdateAsync(Source.Sources).ConfigureAwait(false);
		await RunAddOrUpdateAsync(Classifier.Classifiers).ConfigureAwait(false);
		await RunAddOrUpdateAsync(ProductFlowType.ProductFlowTypes).ConfigureAwait(false);
	}

	private async Task InitializerUsersAsync(CancellationToken cancel)
	{
		if (await _db.UsersInfo.AnyAsync(cancel).ConfigureAwait(false))
			return;

		_logger.LogInformation("Добавление в БД пользователей...");
		await _db.UsersInfo.AddRangeAsync(TestData.Users, cancel).ConfigureAwait(false);
	}

	private async Task InitializerOrganizationsAsync(CancellationToken cancel)
	{
		if (await _db.Organizations.AnyAsync(cancel).ConfigureAwait(false))
			return;

		_logger.LogInformation("Добавление в БД тестовых организаций...");
		await _db.Organizations.AddRangeAsync(TestData.Organizations, cancel).ConfigureAwait(false);
	}

	private async Task InitializerNomenclaturesAsync(CancellationToken cancel)
	{
		if (await _db.Nomenclatures.AnyAsync(cancel).ConfigureAwait(false))
			return;

		_logger.LogInformation("Добавление в БД тестовой номенклатуры...");
		await _db.Nomenclatures.AddRangeAsync(TestData.Nomenclatures, cancel).ConfigureAwait(false);
	}

	private async Task InitializerWarehousesAsync(CancellationToken cancel)
	{
		if (await _db.Warehouses.AnyAsync(cancel).ConfigureAwait(false))
			return;

		_logger.LogInformation("Добавление в БД тестовых складов...");
		await _db.Warehouses.AddRangeAsync(TestData.Warehouses, cancel).ConfigureAwait(false);
	}

	private async Task InitializerPartiesAsync(CancellationToken cancel)
	{
		if (await _db.Parties.AnyAsync(cancel).ConfigureAwait(false))
			return;

		_logger.LogInformation("Добавление в БД партий...");
		await _db.Parties.AddRangeAsync(TestData.Parties, cancel).ConfigureAwait(false);
	}

	private async Task InitializerReceiptsAsync(CancellationToken cancel)
	{
		if (await _db.Receipts.AnyAsync(cancel).ConfigureAwait(false))
			return;

		_logger.LogInformation("Добавление в БД тестовых поступлений...");
		await _db.Receipts.AddRangeAsync(TestData.Receipts, cancel).ConfigureAwait(false);
	}

	private async Task InitializerStockAvailabilitiesAsync(CancellationToken cancel)
	{
		if (await _db.StockAvailabilities.AnyAsync(cancel).ConfigureAwait(false))
			return;

		_logger.LogInformation("Добавление в БД тестовых остатков...");
		await _db.StockAvailabilities.AddRangeAsync(TestData.StockAvailabilities, cancel).ConfigureAwait(false);
	}
}
