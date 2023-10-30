using Microsoft.EntityFrameworkCore;

using Note.API.DAL.Context;
using Note.API.Domain.Note;
using Note.API.Infrastructure.Mappers;
using Note.API.Models.DTO;
using Note.API.Services.Interfaces;

using Service.Common.DTO;
using Service.Common.Interfaces;

namespace Note.API.Services;

public class NotesService : INotesService
{
	private readonly NoteDB _db;
	private readonly ISaveService<NoteDB> _saveService;
	private readonly ILogger<NotesService> _logger;

	private readonly Guid _userId;

	public NotesService(NoteDB dB, ISaveService<NoteDB> saveService, IIdentityService identityService, ILogger<NotesService> logger)
	{
		_db = dB;
		_saveService = saveService;
		_userId = identityService.GetUserIdIdentity() ?? throw new InvalidOperationException($"Пользователь не найден");
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<NoteDto>> GetListAsync(NoteFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.UserNotes
			.Where(u => !u.DeletedDate.HasValue);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.Content.Contains(filter.Search));
		}

		if (filter.ExecutionDate.HasValue)
			query = query.Where(q => q.ExecutionDate == filter.ExecutionDate.Value);

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<NoteDto>();

		var entities = await query
			.OrderByDescending(q => q.IsFix) // сначала нам нужны фиксированные заметки
			.ThenBy(q => q.Sort)
			.Skip(filter.Skip)
			.Take(filter.Take)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = entities.Cast<UserNote>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<NoteDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	public async Task<NoteDto?> GetByIdAsync(Guid id)
	{
		var note = await _db.UserNotes
			.AsNoTracking()
			.SingleOrDefaultAsync(n => !n.DeletedDate.HasValue && n.Id == id)
			.ConfigureAwait(false);

		if (note is null)
		{
			_logger.LogWarning("Заметка с id: {0} не найдена в БД. Операция чтения невозможна.", id);
			return null;
		}

		return note.CreateDto();
	}

	public async Task<Guid> CreateAsync(NoteDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		dto.Sort = await GetSortAsync(dto.IsFix).ConfigureAwait(false);

		var note = dto.CreateEntity(_userId)!;

		await _db.AddAsync(note).ConfigureAwait(false);

		await _saveService.SaveAsync(_db);

		dto.Id = note.Id;

		return dto.Id;
	}

	public async Task<bool> UpdateAsync(NoteDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var note = await _db.UserNotes
			.SingleOrDefaultAsync(n => !n.DeletedDate.HasValue && n.Id == dto.Id)
			.ConfigureAwait(false);

		if (note is null)
		{
			_logger.LogWarning("Заметка с id: {0} не найдена в БД. Операция обновления невозможна.", dto.Id);
			return false;
		}

		if (dto.IsFix != note.IsFix)
		{
			// если различаются значения фиксации, то нам нужно получить новый Sort, тк элемент перекачевал в другую группу,
			// по умолчанию новый элемент группы будет располагаться в конце
			dto.Sort = await GetSortAsync(dto.IsFix).ConfigureAwait(false);
		}

		note.UpdateEntity(dto, _userId);

		_db.Update(note);

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var note = await _db.UserNotes
			.SingleOrDefaultAsync(x => x.Id == id)
			.ConfigureAwait(false);

		if (note is null)
		{
			_logger.LogWarning("Заметка с id: {0} не найдена в БД. Операция удаления невозможна.", id);
			return false;
		}

		note.Sort = -1;
		note.DeletedBy = _userId;
		note.DeletedDate = DateTimeOffset.Now.ToLocalTime();

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> UpdateSortAsync(NoteDto[] dtoArray)
	{
		// с клиента нам придёт массив с последовательностью, которую сделал пользователь, нам надо установить корректную возрастающую сортировку
		// для каждой группы, но тк на клиенте есть пагинация, то к нам будут приходить элементы группами, а значит нам надо всегда находить 
		// минимальный номер сортировки каждой группы
		var groupByIsFix = dtoArray.GroupBy(a => a.IsFix);

		var ids = dtoArray.Select(a => a.Id);

		var notes = await _db.UserNotes
			.Where(u => !u.DeletedDate.HasValue && ids.Contains(u.Id))
			.ToArrayAsync()
			.ConfigureAwait(false);

		if (!notes.Any())
		{
			_logger.LogWarning("Не найдено ни одной заметки в БД с ids: {ids}. Операция обновления сортировки невозможна.", string.Join(";", ids));
			return false;
		}

		foreach (var group in groupByIsFix)
		{
			var sort = group.Select(q => q.Sort).Min();

			foreach (var item in group)
			{
				var note = notes.SingleOrDefault(n => n.Id == item.Id);

				if (note is null)
					continue;

				note.Sort = sort++;
			}
		}

		await _saveService.SaveAsync(_db);

		return true;
	}

	private async Task<int> GetSortAsync(bool isFix)
	{
		if (isFix)
		{
			var lastItem = await _db.UserNotes
				.Where(u => u.IsFix && !u.DeletedDate.HasValue)
				.OrderBy(u => u.Sort)
				.Select(u => new { u.Id, u.Sort })
				.LastOrDefaultAsync()
				.ConfigureAwait(false);

			return lastItem is null ? 0 : (lastItem.Sort + 1);
		}
		else
		{
			var lastItem = await _db.UserNotes
				.Where(u => !u.IsFix && !u.DeletedDate.HasValue)
				.OrderBy(u => u.Sort)
				.Select(u => new { u.Id, u.Sort })
				.LastOrDefaultAsync()
				.ConfigureAwait(false);

			return lastItem is null ? 0 : (lastItem.Sort + 1);
		}
	}
}
