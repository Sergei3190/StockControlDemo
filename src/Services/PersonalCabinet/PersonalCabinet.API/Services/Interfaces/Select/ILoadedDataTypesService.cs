using PersonalCabinet.API.Models.DTO.LoadedDataType;

using Service.Common.DTO.Entities.Base;
using Service.Common.Interfaces;

namespace PersonalCabinet.API.Services.Interfaces.Select;

/// <summary>
/// Сервис типов загруженной информации о пользователе
/// </summary>
public interface ILoadedDataTypesService : ISelectService<NamedEntityDto, LoadedDataTypeFilterDto>
{
}