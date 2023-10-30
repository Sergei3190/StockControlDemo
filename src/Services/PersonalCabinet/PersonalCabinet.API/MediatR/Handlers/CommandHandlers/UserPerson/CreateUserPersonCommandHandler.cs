using MediatR;

using PersonalCabinet.API.DAL.Context;
using PersonalCabinet.API.Domain.Person;
using PersonalCabinet.API.MediatR.Commands.UserPerson;
using PersonalCabinet.API.Services.Interfaces;

using Service.Common.Interfaces;

namespace PersonalCabinet.API.MediatR.CommandHandlers.UserPerson;

public class CreateUserPersonCommandHandler : IRequestHandler<CreateUserPersonCommand, Guid>
{
	private readonly IUserPersonsService _service;
	private readonly PersonalCabinetDB _db;
	private readonly ISaveService<PersonalCabinetDB> _saveService;
	private readonly ILogger<CreateUserPersonCommandHandler> _logger;

	public CreateUserPersonCommandHandler(IUserPersonsService service,
		PersonalCabinetDB db,
		ISaveService<PersonalCabinetDB> saveService,
		ILogger<CreateUserPersonCommandHandler> logger)
	{
		_service = service;
		_db = db;
		_saveService = saveService;
		_logger = logger;	
	}

	public async Task<Guid> Handle(CreateUserPersonCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Dto;

		if (dto is null)
		{
			var error = string.Format("Отсутствуют данные для выполнения команды {0}", typeof(CreateUserPersonCommand));
			throw new ArgumentNullException(error, nameof(dto));
		}

		_logger.LogInformation("Добавление в БД карточки персоны...");
		var cardId = await CreateCardAsync().ConfigureAwait(false);
		_logger.LogInformation("Добавление в БД карточки {cardId} персоны завершено успешно", cardId);

		dto.CardId = cardId;

		return await _service.CreateAsync(dto).ConfigureAwait(false);
	}

	private async Task<Guid> CreateCardAsync()
	{
		var card = new Card() { Id = Guid.NewGuid() };

		await _db.Cards.AddAsync(card).ConfigureAwait(false);

		await _saveService.SaveAsync(_db).ConfigureAwait(false);
		
		return card.Id;
	}
}