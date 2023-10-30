using Service.Common.DTO.Entities.Base;
using Service.Common.Interfaces;

using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.Services.Interfaces.Select;

/// <summary>
/// Сервис элементов справочника номенклатура для выпадающего списка
/// </summary>
public interface ISelectNomenclaturesService : ISelectService<NamedEntityDto, SelectNomenclatureFilterDto>
{
}