using Service.Common.DTO.Entities.Base;
using Service.Common.Interfaces;

using StockControl.API.Models.DTO;

namespace StockControl.API.Services.Interfaces.Select;

/// <summary>
/// Сервис партий товара
/// </summary>
public interface ISelectPartiesService : ISelectService<NamedEntityDto, SelectPartyFilterDto>
{
}