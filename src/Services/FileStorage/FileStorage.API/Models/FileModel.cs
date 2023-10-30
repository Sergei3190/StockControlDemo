using FileStorage.API.Services;

using Service.Common.DTO;

namespace FileStorage.API.Models;

/// <summary>
/// Файл
/// </summary>
public class FileModel
{
	public StorageFileInfo Info { get; set; }

	// для передачи клиенту на случай если в кэше была удалена информация
	public FileDto File { get; set; }

	public byte[] Content { get; set; }
}