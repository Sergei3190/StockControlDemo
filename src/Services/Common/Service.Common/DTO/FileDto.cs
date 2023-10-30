using Service.Common.DTO.Entities.Base;

namespace Service.Common.DTO;

/// <summary>
/// Файл для передачи между микросервисами, извлекаемый из кэша редиса
/// </summary>
public class FileDto : EntityDto
{
    public string FileName { get; set; }

    public string? ContentType { get; set; }

	public long? ContentLength { get; set; }

	public byte[]? Content { get; set; }
}