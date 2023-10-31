# StockControlDemo

The "Stock conrtol" application is "for demonstration purposes" and is NOT INTENDED for a production environment (requires further development), however
it is fault-tolerant, multi-user, extensible, secure and interactive.

The application intentionally uses both material forms and conventional reactive forms.
Screenshots of some of the application's functionality can be viewed at the following link: https://github.com/Sergei3190/StockControlDemo/wiki/Stock-Control-Demo-SPA

The current repository is a clean version of my draft version, which is stored in a closed repository, so the commits are general and have a minimum number of them.

The application runs by default (locally or in docker-compose) with a self-signed certificate generated via openssl.

Run locally without a certificate - see in  https://github.com/Sergei3190/StockControlDemo/tree/main/src/UI/StockControlSPA/wep-stock-control-spa/README.md 

Main app navigation: https://github.com/Sergei3190/StockControlDemo/wiki/Main-app-navigation

During development i used:

- ARCHITECTURE: Microservices
- PATTERNS:
  * BFF (implemented in Web.Bff.StockControl),
  * CQRS, Mediator, (implemented in each business logic microservice)
  * Observer, Publish/Subscribe (Pub/Sub), (implemented in each business logic microservice that uses integration events)
  * Simplified Event Sourcing (implemented in EventBus)
- LANGUAGES: C#, LINQ, TypeScript, JavaScript, HTML, SCSS, CSS, T-SQL, Protobuf, Yaml
- SQL: MSSQL
- NoSQL: MongoDB, Redis, MemoryCache
- ORM: EF Core (Code First + EntityTypeBuilder), Dapper 
- FRAMEWORKS: .NET 7, ASP.NET Web API (REST), Swagger OpenAPI, Worker Service (BackgroundService), ASP.NET MVC (Razor), Duende IdentityServer, gRPC, SignalR, MediatR, Angular 16, nginx, Bootstrap
- SOFTWARE PLATFORMS: Visual Studio 2022, Visual Studio Code, SQL Server Management Studio (SSMS), GitHub, Git Extensions, SEQ, RabbitMQ, Docker, Docker-Compose


SPA UI allows (general points):

- log in and out of the system based on data entered by the user, without contacting external providers (needs improvement)
- register new users with confirmation by email
- reset the password, sending a password reset token to the user's email
- block/unblock user
- receive lists of entities and filter them
- create, update, delete notes, receipts, movements, write-offs with the logic of checking permissible actions implemented on the server
- perform mass deletion of goods movement documents
- data validation
- update notification settings for each user (enabled notifications are displayed at the top of the list)
- receive updated data online (signalR) in terms of receipts, movements, write-offs, in cases where the data has been changed by other users
- enter user data in your personal account
- upload a photo for your personal account
- upload/download user documents in your personal account
- pin important notes to the top of the notes list
- update the sorting of notes by dragging them within the �pinned� and �unpinned� groups
- log errors and main actions
- cancel deletion and editing actions online
- issue notifications to the user via toastr about completed actions,
- save selected elements when changing pages and sorting by columns of UI tables
- monitor online the performance of services through the built-in UI


Since the application is �demonstrative� in nature, then:

- in one document for the movement of goods (receipt, write-off, movement) only one item/item is indicated
- in documents moving and writing off, updating/editing is possible only in terms of number, price and quantity
- each item always has a unique batch - achieved through an automatically generated unique storage warehouse number,
- based on the uniqueness of the batch, the application logic is built
- some checks and filters are missing when creating movements and write-offs (for example, batches are selected at the current moment, and not based on the creation date
write-off/transfer document, but the date and time are checked depending on the receipt date of the batch used
- when creating a transfer/write-off, drop-down lists will be formed based on the current balances in the warehouse, based on
user-selected data in the creation form)
- tests are mainly functional, modular, made for point-by-point demonstration, mainly in Note, EventBus, StockControlSPA
- workflows are also made only for some microservices


���������� "��������� ����" "�����" ���������������� �������� � �� ������������� ��� ���������������� ����� (��������� ���������), ��� �� �����
��� �������� ����������������, ���������������������, �����������, ���������� � �������������.

� ���������� �������� ������������ ��� material forms ��� � ������� ����������. 
������ ��������� ���������������� ���������� ����� ���������� �� ������ : https://github.com/Sergei3190/StockControlDemo/wiki/Stock-Control-Demo-SPA

������� ����������� �������� �������� ��������� ����� ��������� ��������, ������� �������� � �������� �����������, ������� ������� ����� � �� ����������� ���-��.

���������� �� ��������� ����������� (��� ��������, ��� � � docker-compose) � ������� ���������������� �����������, ���������������� ����� openssl.

K�� ��������� �������� ��� ����������� - ������ � https://github.com/Sergei3190/StockControlDemo/tree/main/src/UI/StockControlSPA/wep-stock-control-spa/README.md 

�������� ��������� ����������: https://github.com/Sergei3190/StockControlDemo/wiki/Main-app-navigation

��� ���������� ���� ������������:

- ARCHITECTURE: Microservices
- PATTERNS:
  * BFF (����������� � Web.Bff.StockControl),
  * CQRS, Mediator, (����������� � ������ ������������ ������-������)
  * Observer, Publish/Subscribe (Pub/Sub), (����������� � ������ ������������ ������-������, ������� ���������� �������������� �������)
  * Simplified Event Sourcing (����������� � EventBus)
- LANGUAGES: C#, LINQ, TypeScript, JavaScript, HTML, SCSS, CSS, T-SQL, Protobuf, Yaml
- SQL: MSSQL
- NoSQL: MongoDB, Redis, MemoryCache
- ORM: EF Core (Code First + EntityTypeBuilder), Dapper 
- FRAMEWORKS: .NET 7, ASP.NET Web API (REST), Swagger OpenAPI, Worker Service (BackgroundService), ASP.NET MVC (Razor), Duende IdentityServer, gRPC, SignalR, MediatR, Angular 16, nginx, Bootstrap
- SOFTWARE PLATFORMS: Visual Studio 2022, Visual Studio Code, SQL Server Management Studio (SSMS), GitHub, Git Extensions, SEQ, RabbitMQ, Docker, Docker-Compose


SPA UI ��������� (����� �������):

- ������� � ������� � �������� �� ��, �� ��������� �������� ������������� ������, ��� ��������� � ������� ����������� (����� ���������)
- �������������� ����� ������������� � �������������� �� email
- ���������� ������, � ��������� ������ ������ ������ �� ����� ������������
- �����������/�������������� ������������
- �������� ������ ��������� � ����������� ��
- ���������, ���������, ������� �������, �����������, �����������, �������� � ������������� �� ������� ������� �������� ���������� ��������
- ��������� �������� �������� ���������� �������� ������
- ��������� ������
- ��������� ��������� ����������� ��� ������� ������������ (���������� ����������� ������������ ������ ������)
- �������� ����������� ������ � ������ ������ (signalR) � ������� �����������, �����������, ��������, � ������, ���� ������ ���� �������� ������� ��������������
- ������� ������ ������������ � ������ ��������
- ��������� ���������� ��� ������� ��������
- ���������/��������� ��������� ������������ � ������ ��������
- ���������� ������ ������� � ������� ����� ������ �������
- ��������� ���������� �������, ���� �� �������������� � ������ ����� "������������" � "��������������"
- ���������� ������ � �������� ��������
- �������� �������� �������� � �������������� � ������ ������
- �������� ������������ ����������� ����� toastr � ����������� ���������,
- ��������� ��������� �������� ��� ����� �������� � ���������� � ������� ������� ������ UI
- ����������� � ������ ������ ����������������� �������� ����� ���������� UI


�� ���������� "�����" ���������������� ��������, �� :

- � ����� ��������� �������� ������ (�����������, ��������, �����������) ����������� ������ ���� �������/������������
- � ���������� ����������� � �������� ����������/�������������� �������� ������ � ����� ������, ���� � ����������
- � ������ ������������ ������ ���������� ������ - ����������� �� ���� ������������� ���������������� ����������� ������ ������ ��������,
- �� ��������� ������������ ������ � �������� ������ ����������
- ���������� ��������� �������� � ������� ��� �������� ����������� � �������� (�������� ������ ���������� �� ������� ������, � �� ������ �� ���� ��������
��������� ��������/�����������, �� ����������� �������� ���� � ������� � ����������� �� ���� ����������� ������������ ������
- ��� �������� �����������/�������� ���������� ������ ����� ������������� �� ��������� �������� �� ������ �� ������� ������, ������ ��
��������� ������������� ������ � ����� ��������)
- ����� � �������� ��������������, ��������� ������� ��� ������������ �������, � �������� � Note, EventBus, StockControlSPA
- workflows ���� ������� ������ ��� ��������� �������������