using MediatR;

using StockControl.API.Models.DTO.Organization;

namespace StockControl.API.MediatR.Queries.Organization;

/// <summary>
/// Запрос на получение элемента справочника организаций по id
/// </summary>
public record GetOrganizationByIdQuery(Guid Id) : IRequest<OrganizationDto?>
{
}
