.NET Core Identity ������ IdentityServer4:
https://stackoverflow.com/questions/48355229/net-core-identity-vs-identityserver4#:~:text=ASP.NET%20Identity%20is%20a,on%20and%20API%20access%20control.

https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html

��� ������ ������� IdentityServer ������� ��� ��� ���� ������� ������������, ��� ���� � ������ tempkey.jwk. ��� �� ����� ���������� ���� ���� � ������� ���������� ��������,
�� ����� ������ ������, ���� ��� ���.

�������� ����� ������ � IdentityServer ��������� �������� OAuth 2.0, � ��� ������� � ���� ����� ������������ �������������� HTTP. 
������ � ��� ���� ���������� ���������� ��� ��������� IdentityModel, ������� ������������� �������������� ��������� � ������� � ������������� API.

(OpenID Connect) OIDC (https://learn.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc)

���� �� ��������� ��������� �� ������ ��� ����������� (client.GetDiscoveryDocumentAsync), ��������, �� ����������� https ,
� ���������� ���������� localhost�� �������� ����������. �� ������ ��������� , ����� �������� ����������� ����������. 
��� ����� ������� ������ ���� ���.dotnet dev-certs https --trust

�� Identity.API �� �� ���� �� �������������, � ������ ��������� �������������� �������, �� ����������� EventBus � ���� ��� ��� ������,
� ����� ���� ��������� � Identity.Api.BackgroundTasks, ������� ��� ��� ���������� ���������� �������������� ������� � �������� �� "�������" ���������, 
����� ������� �������� ��������.

������ ��� ��������� SSL:
https://damienbod.com/2016/09/16/full-server-logout-with-identityserver4-and-openid-connect-implicit-flow/

��������� ����� (������������):
https://jwt.ms/

���������� ��������� ������� ��������������:
https://identityserver4.readthedocs.io/en/latest/quickstarts/2_interactive_aspnetcore.html?highlight=External
��� ASP.NET Core ������������ � ���������� Google, Facebook, Twitter, ������� ������ Microsoft � OpenID Connect. 
����� ����, https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers �� ������ ����� ���������� ��� ������ ������ ����������� �������������� .

���������� ������� JavaScript:
https://identityserver4.readthedocs.io/en/latest/quickstarts/4_javascript_client.html

SlidingExpiration � IdentityServer4:
https://stackoverflow.com/questions/63720220/sliding-expiration-with-identity-server-4-and-asp-net-core
https://github.com/aspnet/Security/issues/147

����� ��������� �������� �� ����� ��� ������� �� ����� �� �� ������� ���������� ������� formnovalidate (����� � �� ������� ���������):
https://stackoverflow.com/questions/60440119/asp-net-core-mvc-disable-validation-when-a-specific-button-is-pressed

������������� ������� ������ � �������������� ������ � ASP.NET Core (Email):
https://learn.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-7.0&tabs=visual-studio
���������� ����� ��� ����� ������ � ��������������:
https://code-maze.com/password-reset-aspnet-core-identity/
��� �������� ������������� ����������� ����� � ����������:
https://code-maze.com/email-confirmation-aspnet-core-identity/
+ ������ ����� �� �������:
https://learn.microsoft.com/en-us/aspnet/identity/overview/features-api/account-confirmation-and-password-recovery-with-aspnet-identity
�������� ������� �������� �����:
https://code-maze.com/aspnetcore-send-email/
https://metanit.com/sharp/aspnet5/21.1.php

���������� ������������ � ������� ASP.NET Core Identity:
https://code-maze.com/user-lockout-aspnet-core-identity/


��� ������� ������ ���������� � GMAil:
https://blog.rebex.net/gmail-using-app-passwords

Health Check Middleware (���������� ����������������� ����������):
https://metanit.com/sharp/aspnet6/18.1.php
https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0

��� ��������� �������� ��������, ��������, ������ ��� ������ ��� �����, ���� � ������� ������� ������ ����������� � ���������� ������ ����� ������:
https://github.com/nazmul1985/health-check-dotnet-core

�����!!! 
���� "launchBrowser": false, �� ��� ����� � �������� ������� ��� �������� ��������, � ��� ������ �� ������, ������ �� �����, ����� ������� true

��������� �������� �������� ����� ��������� � ������� ���� ����� ������������ � ���� json, ����� ��������� ����������
� �������� ����� � ����������� ���������������� ����������
��� ��� ���������, ������ ����:
https://imar.spaanjaars.com/611/implementing-health-checks-in-aspnet-core
https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health

�������� ���������� �� ������������ �������� ����������:
https://medium.com/aspnetrun/microservices-monitoring-with-health-checks-using-watchdog-6b16fdae0349

 // ����� ����������� ����������� ������� ��� � ��������� �����, �� � �� ������������, ��������� �� ������, ��������� � ���� ���� � ������������������ 
 // � ����� ������� identity : https://stackoverflow.com/questions/60962172/getauthorizationcontextasyncreturnurl-of-iidentityserverinteractionservice-ret
var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

���������� ��� ���������� �������� � identity:
https://stackoverflow.com/questions/56442402/identityserver4-redirect-to-login-action-with-client-information

������ ��������� � js ����� ������ js ������ � ������� ������:
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

WebPack ��� ����������� ���������� �� ���� ������ ������
https://webpack.js.org/configuration/dev-server/

DateOnly TimeOnly � ��� �������� � ���� ���� ����� � ��
https://code-maze.com/csharp-dateonly-timeonly/
��� ���������������
https://code-maze.com/csharp-map-dateonly-timeonly-types-to-sql/

FluentApi:
https://www.entityframeworktutorial.net/code-first/configure-property-mappings-using-fluent-api.aspx

MediatR:
https://code-maze.com/cqrs-mediatr-in-aspnet-core/
���� ��� ���� ������� : �������� (��� �� ���������� � ��������� � ������ ������������, ���������� ���������� � ��������) � �������������� - ������� ������� ��������� �� ���������
�������� ������� � ������������ � ���� ���������, ��� ������� �� ����������� ��������� �������. 
��� ������������ �������� ������� ���������� MediatR, � SignalR ���������� ��� ������ � ��������������� ���������, ����� �� ��������� ���� ��������/������������� 
��� ����������.

���������� ����� CSP (��� �������� ���������� ����� ���������):
https://content-security-policy.com/
