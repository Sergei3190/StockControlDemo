using Service.Common.DTO.Entities.Base;
using Service.Common.Interfaces;

using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.Services.Interfaces.Select;

/// <summary>
/// Сервис элементов справочника склады для выпадающего списка
/// </summary>
public interface ISelectWarehousesService : ISelectService<NamedEntityDto, SelectWarehouseFilterDto>
{
}