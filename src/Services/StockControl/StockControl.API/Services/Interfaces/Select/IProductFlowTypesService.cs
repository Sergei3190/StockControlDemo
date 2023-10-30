using Service.Common.DTO.Entities.Base;
using Service.Common.Interfaces;

using StockControl.API.Models.DTO.ProductFlowType;

namespace StockControl.API.Services.Interfaces.Select;

/// <summary>
/// Сервис типов движений товара
/// </summary>
public interface IProductFlowTypesService : ISelectService<NamedEntityDto, ProductFlowTypeFilterDto>
{
}