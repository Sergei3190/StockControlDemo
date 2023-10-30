using Service.Common.Interfaces;

using StockControl.API.Models.DTO.Moving;

namespace StockControl.API.Services.Interfaces.ProductFlow;

/// <summary>
/// Сервис перемещений
/// </summary>
public interface IMovingsService : ICrudService<MovingDto, MovingFilterDto>, IBulkDeleteService, IProductFlowInfoService
{
}