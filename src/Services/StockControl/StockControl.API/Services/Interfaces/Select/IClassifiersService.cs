using Service.Common.Interfaces;

using StockControl.API.Models.DTO.Classifier;

namespace StockControl.API.Services.Interfaces.Select;

/// <summary>
/// Сервис справочников
/// </summary>
public interface IClassifiersService : ISelectService<ClassifierDto, ClassifierFilterDto>
{
}