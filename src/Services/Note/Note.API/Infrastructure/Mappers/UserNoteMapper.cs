using System.Runtime.CompilerServices;

using Google.Protobuf.Collections;

using GrpcNote;

using Note.API.Domain.Note;
using Note.API.Models.DTO;

namespace Note.API.Infrastructure.Mappers;

public static class UserNoteMapper
{
	public static UserNote? CreateEntity(this NoteDto dto, Guid userId) => dto is null
		? null
		: new UserNote() 
		{ 
			Id = dto.Id, 
			Content = dto.Content, 
			IsFix = dto.IsFix,
			Sort = dto.Sort,
			ExecutionDate = dto.ExecutionDate,
			UserId = userId,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this UserNote entity, NoteDto dto, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.Content = dto.Content;
		entity.IsFix = dto.IsFix;
		entity.Sort = dto.Sort;
		entity.ExecutionDate = dto.ExecutionDate;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static NoteDto? CreateDto(this UserNote entity) => entity is null
		? null
		: new NoteDto()
		{
			Id = entity.Id,
			Content = entity.Content,
			IsFix = entity.IsFix,
			Sort = entity.Sort,
			ExecutionDate = entity.ExecutionDate
		};

	public static NoteDto? CreateDto(this NoteArrayItemRequest entity) => entity is null
		? null
		: new NoteDto()
		{
			Id = Guid.Parse(entity.Id),
			Content = entity.Content,
			IsFix = entity.IsFix,
			Sort = entity.Sort,
			ExecutionDate = entity.ExecutionDate is null ? null : DateOnly.Parse(entity.ExecutionDate)
		};
}
