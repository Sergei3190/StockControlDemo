using Service.Common.Interfaces;
using StockControl.API.Models.DTO.WriteOff;

namespace StockControl.API.Services.Interfaces.ProductFlow;

/// <summary>
/// Сервис списаний
/// </summary>
public interface IWriteOffsService : ICrudService<WriteOffDto, WriteOffFilterDto>, IBulkDeleteService, IProductFlowInfoService
{
}