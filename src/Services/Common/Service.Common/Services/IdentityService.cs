using Microsoft.AspNetCore.Http;

using Service.Common.Interfaces;

namespace Service.Common.Services;

public class IdentityService : IIdentityService
{
	private readonly IHttpContextAccessor _context;

	public IdentityService(IHttpContextAccessor context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public Guid? GetUserIdIdentity()
	{
		if (_context.HttpContext is null)
			return null;

		// sub - он же Id пользователя в системе Identity
		// id пользователя не желательно передавать в DTO, поэтому мы его извлекаем на уровне сервера и добавляем в бд к сохраняемому объекту, если надо
		var sub = _context.HttpContext!.User.FindFirst("sub")!.Value;

		if (Guid.TryParse(sub, out Guid userId))
			return userId;

		return null;
	}

	public string GetUserNameIdentity()
	{
		return _context.HttpContext!.User.FindFirst("name")?.Value!;
	}
}
