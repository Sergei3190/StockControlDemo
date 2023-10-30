using IntegrationEventLogEF;

using Microsoft.EntityFrameworkCore;

using Note.API.DAL.Context;
using Note.API.Domain.Note;

using Service.Common.DTO;
using Service.Common.Entities.App;
using Service.Common.Interfaces;

namespace Note.API.Services;

public class DbInitializerService : IDbInitializerService
{
	private const string DbName = "Note";

    private readonly NoteDB _db;
    private readonly IntegrationEventLogContext _eventLogContext;
    private readonly ILogger<DbInitializerService> _logger;

    public DbInitializerService(
        NoteDB db,
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
					await InitializerNotesAsync(cancel).ConfigureAwait(false);

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

	private async Task InitializerUsersAsync(CancellationToken cancel)
	{
        if (await _db.UsersInfo.AnyAsync(cancel).ConfigureAwait(false))
            return;

		_logger.LogInformation("Добавление в БД тестовых пользователей...");
		await _db.UsersInfo.AddRangeAsync(TestData.Users, cancel).ConfigureAwait(false);
	}

	private async Task InitializerNotesAsync(CancellationToken cancel)
	{
		if (await _db.UserNotes.AnyAsync(cancel).ConfigureAwait(false))
            return;

		_logger.LogInformation("Добавление в БД тестовых заметок...");
		await _db.UserNotes.AddRangeAsync(TestData.Notes, cancel).ConfigureAwait(false);
	}

	private async Task InitializerStaticDataAsync(CancellationToken cancel = default)
    {
        _logger.LogInformation("Инициализация статических данных {0} БД...", DbName);

        // чтобы выполнить в с отказоустойчивостью в рамках транзакции
        var strategy = _db.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _db.Database.BeginTransactionAsync(cancel).ConfigureAwait(false);

            await AddOrUpdateSourcesAsync().ConfigureAwait(false);

            await _db.SaveChangesAsync().ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);
        })
            .ConfigureAwait(false);

        _logger.LogInformation("Инициализация статических данных {0} БД выполнена успешно", DbName);
    }

    private async Task AddOrUpdateSourcesAsync()
    {
        var appItems = Source.Sources;

        if (await _db.Sources.AnyAsync().ConfigureAwait(false))
        {
            var sourcesDB = await _db.Sources.ToArrayAsync().ConfigureAwait(false);

            // обновляем активность и добавляем новые элементы
            foreach (var item in appItems)
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
            await _db.AddRangeAsync(appItems).ConfigureAwait(false);
    }
}
