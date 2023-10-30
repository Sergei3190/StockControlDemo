using Note.API.Models.DTO;

using Service.Common.Interfaces;

namespace Note.API.Services.Interfaces;

/// <summary>
/// Сервис заметок
/// </summary>
public interface INotesService : ICrudService<NoteDto, NoteFilterDto>
{
	Task<bool> UpdateSortAsync(NoteDto[] dtoArray);
}