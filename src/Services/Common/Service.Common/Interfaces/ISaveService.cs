using Microsoft.EntityFrameworkCore;

namespace Service.Common.Interfaces;

/// <summary>
/// Интерфейс сохранения сущности бд
/// </summary>
public interface ISaveService<TContextDb> where TContextDb : DbContext
{
    Task<int> SaveAsync(TContextDb context, CancellationToken token = default);
}
