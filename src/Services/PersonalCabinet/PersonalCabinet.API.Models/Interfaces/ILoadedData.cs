using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace PersonalCabinet.API.Models.Interfaces;

/// <summary>
/// Загруженные пользователем данные о персоне
/// </summary>
public interface ILoadedData
{
    Guid CardId { get; set; }

    /// <summary>
    /// Загруженный пользователем файл
    /// </summary>
    FileDto File { get; set; }

    /// <summary>
    /// Тип загруженных данных
    /// </summary>
    NamedEntityDto LoadedDataType { get; set; }
}