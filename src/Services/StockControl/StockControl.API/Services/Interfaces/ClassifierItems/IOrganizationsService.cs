using Service.Common.Interfaces;

using StockControl.API.Models.DTO.Organization;

namespace StockControl.API.Services.Interfaces.ClassifierItems;

/// <summary>
/// Сервис организаций
/// </summary>
public interface IOrganizationsService : ICrudService<OrganizationDto, OrganizationFilterDto>, IBulkDeleteService, IClassifierItemInfoService
{
}