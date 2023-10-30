using Service.Common.Interfaces;
using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.Services.Interfaces.ClassifierItems;

/// <summary>
/// Сервис номенклатуры
/// </summary>
public interface INomenclaturesService : ICrudService<NomenclatureDto, NomenclatureFilterDto>, IBulkDeleteService, IClassifierItemInfoService
{
}