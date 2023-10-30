using Service.Common.Interfaces;

using StockControl.API.Models.DTO;
using StockControl.API.Models.DTO.Party;

namespace StockControl.API.Services.Interfaces.ProductFlow;

/// <summary>
/// Сервис партий
/// </summary>
public interface IPartiesService : ICrudService<PartyDto, PartyFilterDto>, IBulkDeleteService
{
}