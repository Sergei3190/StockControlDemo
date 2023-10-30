using FileStorage.API.Models;
using FileStorage.API.Services;

using Service.Common.DTO;

namespace FileStorage.API.Infrastructure.Mappers;

public static class StorageFileInfoMapper
{
	public static FileDto? CreateFileDto(this FileModel file) => file is null 
		? null
		: new FileDto()
		{
			Id = file.Info.Id,
			FileName = file.Info.OriginalName,
			ContentType = file.Info.ContentType,
			ContentLength = file.Info.ContentLength,
			Content = file.Content
		};

	public static StorageFileInfo? CreateEntity(this IFormFile file, string originHost, string authorizationHeader) => file is null
		? null
		: new StorageFileInfo()
		{
			OriginalName = file.FileName,
			ContentType = file.ContentType,
			ContentLength = file.Length,
			OriginHost = originHost,
			AuthorizationHeader = authorizationHeader
		};
}
