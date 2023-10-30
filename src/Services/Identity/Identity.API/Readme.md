.NET Core Identity против IdentityServer4:
https://stackoverflow.com/questions/48355229/net-core-identity-vs-identityserver4#:~:text=ASP.NET%20Identity%20is%20a,on%20and%20API%20access%20control.

https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html

При первом запуске IdentityServer создаст для вас ключ подписи разработчика, это файл с именем tempkey.jwk. Вам не нужно возвращать этот файл в систему управления версиями,
он будет создан заново, если его нет.

Конечная точка токена в IdentityServer реализует протокол OAuth 2.0, и для доступа к нему можно использовать необработанный HTTP. 
Однако у нас есть клиентская библиотека под названием IdentityModel, которая инкапсулирует взаимодействие протокола в простой в использовании API.

(OpenID Connect) OIDC (https://learn.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc)

Если вы получаете сообщение об ошибке при подключении (client.GetDiscoveryDocumentAsync), возможно, вы используете https ,
а сертификат разработки localhostне является доверенным. Вы можете запустить , чтобы доверять сертификату разработки. 
Это нужно сделать только один раз.dotnet dev-certs https --trust

Тк Identity.API ни на кого не подписывается, а только испускает интеграционные события, то настраивать EventBus у него нам нет смысла,
а смысл есть настроить в Identity.Api.BackgroundTasks, который как раз занимается публикацие интеграционных событий и подписан на "мертвые" сообщения, 
чтобы сделать доставку повторно.

Статья как настроить SSL:
https://damienbod.com/2016/09/16/full-server-logout-with-identityserver4-and-openid-connect-implicit-flow/

Проверить токен (декодировать):
https://jwt.ms/

Добавление поддержки внешней аутентификации:
https://identityserver4.readthedocs.io/en/latest/quickstarts/2_interactive_aspnetcore.html?highlight=External
Сам ASP.NET Core поставляется с поддержкой Google, Facebook, Twitter, учетной записи Microsoft и OpenID Connect. 
Кроме того, https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers вы можете найти реализации для многих других провайдеров аутентификации .

Добавление клиента JavaScript:
https://identityserver4.readthedocs.io/en/latest/quickstarts/4_javascript_client.html

SlidingExpiration в IdentityServer4:
https://stackoverflow.com/questions/63720220/sliding-expiration-with-identity-server-4-and-asp-net-core
https://github.com/aspnet/Security/issues/147

ЧТОБЫ ОТКЛЮЧИТЬ ПРОВЕРКУ НА ФОРМЕ ПРИ НАЖАТИИ НА КАКОЙ ЛИ БО ЭЛЕМЕНТ ИСПОЛЬЗУЕМ АТРИБУТ formnovalidate (можно и на сервере отключить):
https://stackoverflow.com/questions/60440119/asp-net-core-mvc-disable-validation-when-a-specific-button-is-pressed

Подтверждение учетной записи и восстановление пароля в ASP.NET Core (Email):
https://learn.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-7.0&tabs=visual-studio
Попонятнее здесь про сброс пароля и восстановление:
https://code-maze.com/password-reset-aspnet-core-identity/
Про отправку подтверждения электронной почты с вложениями:
https://code-maze.com/email-confirmation-aspnet-core-identity/
+ единый выход из системы:
https://learn.microsoft.com/en-us/aspnet/identity/overview/features-api/account-confirmation-and-password-recovery-with-aspnet-identity
Создание сервиса отправки писем:
https://code-maze.com/aspnetcore-send-email/
https://metanit.com/sharp/aspnet5/21.1.php

Блокировка пользователя с помощью ASP.NET Core Identity:
https://code-maze.com/user-lockout-aspnet-core-identity/


Как создать пароль приложения в GMAil:
https://blog.rebex.net/gmail-using-app-passwords

Health Check Middleware (Мониторинг работоспособности приложения):
https://metanit.com/sharp/aspnet6/18.1.php
https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0

Для настройки проверки здоровья, например, редиса или рэбита или монго, надо в конфиге указать строки подключения и установить нужные нугет пакеты:
https://github.com/nazmul1985/health-check-dotnet-core

ВАЖНО!!! 
Если "launchBrowser": false, то при вводе в браузере адресов для проверки злоровья, у нас ничего не выйдет, ответа не будет, нужно ставить true

результат проверки здоровья после обращения к данному урлу будет отображаться в виде json, можно настроить визуальную
и красивую часть в специальном пользовательском интерфейсе
как его настроить, читаем ниже:
https://imar.spaanjaars.com/611/implementing-health-checks-in-aspnet-core
https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health

Создание интерфейса по отслеживанию здоровья приложений:
https://medium.com/aspnetrun/microservices-monitoring-with-health-checks-using-watchdog-6b16fdae0349

 // нужно обязательно прокидывать оратный урл в указанный метод, тк в нём определяется, относится ли клиент, указанный в этом урле к зарегестрированным 
 // в нашей системе identity : https://stackoverflow.com/questions/60962172/getauthorizationcontextasyncreturnurl-of-iidentityserverinteractionservice-ret
var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

объяснение как происходит редирект в identity:
https://stackoverflow.com/questions/56442402/identityserver4-redirect-to-login-action-with-client-information

Пример установки в js через чистый js показа и скрытия пароля:
//$(function () {
//    const togglePassword = document.querySelector('#togglePassword');
//    const password = document.querySelector('#Password');

//    togglePassword.addEventListener('click', function (e) {
//        // toggle the type attribute
//        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
//        password.setAttribute('type', type);
//        // toggle the eye slash icon
//        this.classList.toggle('fa-eye-slash');
//    });
//});

WebPack для оптимизации разработки за счет сжатия файлов
https://webpack.js.org/configuration/dev-server/

DateOnly TimeOnly и как работать с ними втом числе в бд
https://code-maze.com/csharp-dateonly-timeonly/
как преобразовывать
https://code-maze.com/csharp-map-dateonly-timeonly-types-to-sql/

FluentApi:
https://www.entityframeworktutorial.net/code-first/configure-property-mappings-using-fluent-api.aspx

MediatR:
https://code-maze.com/cqrs-mediatr-in-aspnet-core/
Есть два вида событий : доменные (что то происходит с сущностью в рамках микросервиса, добавление обновление и удаление) и интеграционные - события которые генерятся на основании
доменных событий и отправляются в шину сообщений, как правило из обработчика доменного события. 
Для формирования доменных событий используем MediatR, а SignalR используем для работы с интеграционными событиями, чтобы их отправить всем клиентам/пользователям 
веб приложения.

Встроенные стили CSP (как включать встроенные стили безопасно):
https://content-security-policy.com/
