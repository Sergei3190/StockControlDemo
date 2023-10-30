using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Service.Common.Converters.DB;

/// <summary>
/// Помогает EF сопоставить тип TimeOnly .Net с типом time Sql
/// </summary>
public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyConverter() : base(
            d => d.ToTimeSpan(),
            d => TimeOnly.FromTimeSpan(d))
    { }
}
