using Service.Common.DTO.Entities.Base;
using Service.Common.Interfaces;

using StockControl.API.Models.DTO.Organization;

namespace StockControl.API.Services.Interfaces.Select;

/// <summary>
/// Сервис элементов справочника организации для выпадающего списка
/// </summary>
public interface ISelectOrganizationsService : ISelectService<NamedEntityDto, SelectOrganizationFilterDto>
{
}