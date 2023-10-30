using Service.Common.Interfaces;

using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.Services.Interfaces.ClassifierItems;

/// <summary>
/// Сервис складов
/// </summary>
public interface IWarehousesService : ICrudService<WarehouseDto, WarehouseFilterDto>, IBulkDeleteService, IClassifierItemInfoService
{
}