namespace Service.Common.Interfaces;

/// <summary>
/// Интерфейс по работе с данными системы Identity
/// </summary>
public interface IIdentityService
{
    Guid? GetUserIdIdentity();
    string GetUserNameIdentity();
}