using System;

using IntegrationEventLogEF;

using Microsoft.EntityFrameworkCore;

using PersonalCabinet.API.DAL.Context;
using PersonalCabinet.API.Domain;
using PersonalCabinet.API.Domain.Person;
using PersonalCabinet.API.Models.DTO.UserPerson;
using PersonalCabinet.API.Services.Interfaces;

using Service.Common.DTO;
using Service.Common.Entities.App;
using Service.Common.Entities.Base;
using Service.Common.Interfaces;

namespace PersonalCabinet.API.Services;

public class DbInitializerService : IDbInitializerService
{
    private const string DbName = "Personal Cabinet";

    private readonly PersonalCabinetDB _db;
    private readonly IntegrationEventLogContext _eventLogContext;
    private readonly ILogger<DbInitializerService> _logger;

    public DbInitializerService(
        PersonalCabinetDB db,
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
					await InitializerPersonsAsync(cancel).ConfigureAwait(false);

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
        await RunAddOrUpdateAsync(LoadedDataType.LoadedDataTypes).ConfigureAwait(false);
    }

	private async Task InitializerUsersAsync(CancellationToken cancel)
	{
		if (await _db.UsersInfo.AnyAsync(cancel).ConfigureAwait(false))
			return;

		_logger.LogInformation("Добавление в БД тестовых пользователей...");
		await _db.UsersInfo.AddRangeAsync(TestData.Users, cancel).ConfigureAwait(false);
	}

	private async Task InitializerPersonsAsync(CancellationToken cancel)
	{
		var persons = TestData.Users.Cast<UserInfo>().Select(u => new UserPerson() 
        {
			LastName = u.Name,
			FirstName = u.Name,
			MiddleName = u.Name,
			Card = new Card(),
			UserId = u.Id
		});

		_logger.LogInformation("Добавление в БД тестовых персон...");
		await _db.UserPersons.AddRangeAsync(persons, cancel).ConfigureAwait(false);
	}
}
