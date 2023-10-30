using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Models.DTO;

/// <summary>
/// DTO отправки письма на электронную почту пользователя
/// </summary>
/// <param name="Action">Наименование действия</param>
/// <param name="Controller">Наименование контроллера действия</param>
/// <param name="Url">Ссылка, которая будет отправлено на электронную почту пользователя</param>
/// <param name="ClientId">Идентификатор клиента в системе Identity</param>
/// <param name="Scheme">Http cхема запроса клиента</param>
public record SendEmailDto(string Action, string Controller, IUrlHelper Url, string ClientId, string Scheme);