using FileParser.Service;

using IntegrationEventLogEF;

using Microsoft.EntityFrameworkCore;

using Notification.API.DAL.Context;
using Notification.API.DAL.Scripts;
using Notification.API.Domain;
using Notification.API.Domain.Notice;

using Service.Common.DTO;
using Service.Common.Entities.App;
using Service.Common.Entities.Base;
using Service.Common.Interfaces;

namespace Notification.API.Services;

public class DbInitializerService : IDbInitializerService
{
	private const string DbName = "Notification";

	private readonly NotificationDB _db;
	private readonly IntegrationEventLogContext _eventLogContext;
	private readonly IFileParserService _fileParserService;
	private readonly ILogger<DbInitializerService> _logger;

	public DbInitializerService(
		NotificationDB db,
		IntegrationEventLogContext eventLogContext,
		IFileParserService fileParserService,
		ILogger<DbInitializerService> logger)
	{
		_db = db;
		_eventLogContext = eventLogContext;
		_fileParserService = fileParserService;
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

			var basePath = AppDomain.CurrentDomain.BaseDirectory;

			if (!dto.IsFunctionalTest)
				await InitializerStaticDataAsync(cancel).ConfigureAwait(false);

			await InitializerScriptsAsync(basePath, cancel).ConfigureAwait(false);

			if (dto.AddTestData)
			{
				var strategy = _db.Database.CreateExecutionStrategy();

				await strategy.ExecuteAsync(async () =>
				{
					await using var transaction = await _db.Database.BeginTransactionAsync(cancel).ConfigureAwait(false);

					_logger.LogInformation("Инициализация БД тестовыми данными...");
					await InitializerUsersAsync(cancel).ConfigureAwait(false);

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
		await RunAddOrUpdateAsync(NotificationType.NotificationTypes).ConfigureAwait(false);
	}

	private async Task InitializerScriptsAsync(string basePath, CancellationToken cancel = default)
	{
		async Task RunScriptsAsync(string basePath, IEnumerable<string> paths)
		{
			var fullPaths = _fileParserService.GetFullPaths(paths, basePath);

			var scriptStrings = await _fileParserService.GetFilesToStringsAsync(fullPaths).ConfigureAwait(false);

			foreach (var script in scriptStrings)
			{
				// без транзакции идёт, но у нас есть транзакция выше
				await _db.Database.ExecuteSqlRawAsync(script).ConfigureAwait(false);
			}
		}

		_logger.LogInformation("Инициализация табличных типов {0} БД...", DbName);
		await RunScriptsAsync(basePath, ScriptFilePaths.TableTypes).ConfigureAwait(false);
		_logger.LogInformation("Инициализация табличных типов {0} БД выполнена успешно", DbName);

		_logger.LogInformation("Инициализация хранимых процедур {0} БД...", DbName);
		await RunScriptsAsync(basePath, ScriptFilePaths.Procedures).ConfigureAwait(false);
		_logger.LogInformation("Инициализация хранимых процедур {0} БД выполнена успешно", DbName);

		_logger.LogInformation("Инициализация тригеров {0} БД...", DbName);
		await RunScriptsAsync(basePath, ScriptFilePaths.Triggers).ConfigureAwait(false);
		_logger.LogInformation("Инициализация тригеров {0} БД выполнена успешно", DbName);

		_logger.LogInformation("Запуск тестов скриптов {0} БД...", DbName);
		await RunScriptsAsync(basePath, ScriptFilePaths.Tests).ConfigureAwait(false);
		_logger.LogInformation("Тесты скриптов {0} БД выполнены успешно", DbName);
	}

	private async Task InitializerUsersAsync(CancellationToken cancel)
	{
		if (await _db.UsersInfo.AnyAsync(cancel).ConfigureAwait(false))
			return;

		_logger.LogInformation("Добавление в БД тестовых пользователей...");
		await _db.UsersInfo.AddRangeAsync(TestData.Users, cancel).ConfigureAwait(false);
	}
}
