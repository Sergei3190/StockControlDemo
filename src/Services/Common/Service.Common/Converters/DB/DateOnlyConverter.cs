using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Service.Common.Converters.DB;

/// <summary>
/// Помогает EF сопоставить тип DateOnly .Net с типом date Sql
/// </summary>
public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d))
    { }
}
