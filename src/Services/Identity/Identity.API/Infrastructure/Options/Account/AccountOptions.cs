namespace Identity.API.Infrastructure.Options.Account
{
	public class AccountOptions
	{
		public static bool AllowLocalLogin = true;
		public static bool AllowRememberLogin = true;
		public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(10);

		/// <summary>
		/// Показать подсказку выхода из системы
		/// </summary>
		public static bool ShowLogoutPrompt = true;

		/// <summary>
		/// Автоматическое перенаправление после выхода из системы
		/// </summary>
		public static bool AutomaticRedirectAfterSignOut = false;

		public static string InvalidCredentialsErrorMessage = "Неправильный логин или пароль.";
		public static string UserLockedOutErrorMessage = "Превышено максимальное количество попыток входа. Пользователь заблокирован." +
			" Следуйте инструкции, выcланной на Вашу почту.";
		public static string AttemptsLeftErrorMessage = "Осталось попыток входа: {0}";
		public static string InvalidLoginAttempt = "Неверная попытка входа в систему.";
		public static string DontEmailConfirmend = "Для входа в систему необходимо иметь подтвержденный адрес электронной почты." +
			" Токен подтверждения был повторно отправлен на вашу учетную запись электронной почты.";
	}
}
