using Email.Service;

using Identity.API.DAL.Context;
using Identity.API.Domain.Entities;

using IntegrationEventLogEF;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Service.Common.DTO;
using Service.Common.Interfaces;

namespace Identity.API.Services;

public class DbInitializerService : IDbInitializerService
{
    // нужен для созхдания тестовых данных в микросервисах при запуске проекта
    private const string TestAdminId = "0E97468A-9710-48FB-B6C4-FCEB9C17D6D5";
    private const string DbName = "Identity";

    private readonly SemaphoreSlim _semaphore;

	private readonly IdentityDB _db;
    private readonly IntegrationEventLogContext _eventLogContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly EmailConfiguration _emailConfiguration;
    private readonly ILogger<DbInitializerService> _logger;

    public DbInitializerService(
        IdentityDB db,
        IntegrationEventLogContext eventLogContext,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptionsSnapshot<EmailConfiguration> options,
        ILogger<DbInitializerService> logger)
    {
		// говорим, только один поток сможет получить доступ к ресурсу или пулу ресурсов определенных в рамках симафора до тех пор, 
		// пока не будет закончена работа, те не вызван метод _semaphore.Release();
		_semaphore = new SemaphoreSlim(1); 
		_db = db;
        _eventLogContext = eventLogContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _emailConfiguration = options.Value;
        _logger = logger;
    }

    public async Task InitializeAsync(DbInitializerDto dto, CancellationToken cancel = default)
    {
        try
        {
			await _semaphore.WaitAsync();

			_logger.LogInformation("Инициализация {0} БД...", DbName);

			if (dto.Recreate)
				await RemoveAsync(cancel).ConfigureAwait(false);

			_logger.LogInformation("Применение миграций {0} БД...", DbName);
			await _db.Database.MigrateAsync(cancel).ConfigureAwait(false);
			await _eventLogContext.Database.MigrateAsync(cancel).ConfigureAwait(false);
			_logger.LogInformation("Применение миграций {0} БД выполнено", DbName);

			if (!dto.IsFunctionalTest)
				await InitializerIdentitiesAsync(dto.AddTestData, cancel).ConfigureAwait(false);

			_logger.LogInformation("Инициализация {0} БД выполнена успешно", DbName);
		}
        catch (Exception ex)
        {
            _logger.LogError("Произошла ошибка при инициализации {0} БД: {1}", DbName, ex);
            throw;
        }
        finally 
        { 
            // высвобождаем поток после его использования, чтобы другой смог обратится к ресурсам используемым при инициализации бд
            _semaphore.Release(); 
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

    private async Task InitializerIdentitiesAsync(bool addTestData, CancellationToken cancel = default)
    {
        async Task CheckRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                _logger.LogInformation("Роль {0} существует в {1} БД", roleName, DbName);
            else
            {
                _logger.LogInformation("Роль {0} отсутствует в {1} БД. Создаем...", roleName, DbName);
                await _roleManager.CreateAsync(new Role() { Name = roleName }).ConfigureAwait(false);
                _logger.LogInformation("Роль {0} успешно создана в {1} БД", roleName, DbName);
            }
        }

        await CheckRoleAsync(Role.Administrations).ConfigureAwait(false);
        await CheckRoleAsync(Role.Users).ConfigureAwait(false);

        if (await _userManager.FindByNameAsync(User.Administrator) is null)
        {
            _logger.LogInformation("Пользователь {0} отсутствует в {1} БД. Создаем...", User.Administrator, DbName);

            var admin = new User()
            {
                Id = addTestData ? Guid.Parse(TestAdminId) : Guid.Empty,
                UserName = User.Administrator,
                // тк мы используем для теста ту же почту, что и у админа, то добавим ложный знак, в реальности почта должна быть уникальна
                Email = "admin" + _emailConfiguration.From,
                EmailConfirmed = true,
            };

            var create_result = await _userManager.CreateAsync(admin, User.AdminPassword).ConfigureAwait(false);

            if (create_result.Succeeded)
            {
                _logger.LogInformation("Пользователь {0} успешно создан в {1} БД. Присваеваем ему роль администратора...", User.Administrator, DbName);

                await _userManager.AddToRoleAsync(admin, Role.Administrations).ConfigureAwait(false);

                _logger.LogInformation("Пользователю {0} присвоена роль администратора", User.Administrator);
            }
            else
            {
                var errors = create_result.Errors.Select(e => e.Description);

                var error_message = string.Join(", ", errors);

                throw new InvalidOperationException($"Невозможно создать {User.Administrator}. Ошибка {error_message}");
            }
        }
        else
            _logger.LogInformation("Пользователь {0} существует в {1} БД.", User.Administrator, DbName);

    }
}
