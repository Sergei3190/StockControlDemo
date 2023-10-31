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
- update the sorting of notes by dragging them within the “pinned” and “unpinned” groups
- log errors and main actions
- cancel deletion and editing actions online
- issue notifications to the user via toastr about completed actions,
- save selected elements when changing pages and sorting by columns of UI tables
- monitor online the performance of services through the built-in UI


Since the application is “demonstrative” in nature, then:

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


Приложение "Складской учет" "носит" демонстрационный характер и НЕ ПРЕДНАЗНАЧЕНО для производственной среды (требуется доработка), тем не менее
оно является отказоустойчивым, многопользовательским, расширяемым, безопасным и интерактивным.

В приложении намерено используются как material forms так и обычные реактивные. 
Скрины некоторой функциональности приложения можно посмотреть по ссылке : https://github.com/Sergei3190/StockControlDemo/wiki/Stock-Control-Demo-SPA

Текущий репозиторий является чистовым вариантом моего чернового варианта, который хранится в закрытом репозитории, поэтому коммиты общие и их минимальное кол-во.

Приложение по умолчанию запускается (как локально, так и в docker-compose) с помощью самоподписанного сертификата, сгенерированного через openssl.

Kак запустить локально без сертификата - смотри в https://github.com/Sergei3190/StockControlDemo/tree/main/src/UI/StockControlSPA/wep-stock-control-spa/README.md 

Основная навигация приложения: https://github.com/Sergei3190/StockControlDemo/wiki/Main-app-navigation

При разработке были использованы:

- ARCHITECTURE: Microservices
- PATTERNS:
  * BFF (реализуется в Web.Bff.StockControl),
  * CQRS, Mediator, (реализуются в каждом микросервисе бизнес-логики)
  * Observer, Publish/Subscribe (Pub/Sub), (реализуются в каждом микросервсие бизнес-логики, которые используют интеграционные события)
  * Simplified Event Sourcing (реализуется в EventBus)
- LANGUAGES: C#, LINQ, TypeScript, JavaScript, HTML, SCSS, CSS, T-SQL, Protobuf, Yaml
- SQL: MSSQL
- NoSQL: MongoDB, Redis, MemoryCache
- ORM: EF Core (Code First + EntityTypeBuilder), Dapper 
- FRAMEWORKS: .NET 7, ASP.NET Web API (REST), Swagger OpenAPI, Worker Service (BackgroundService), ASP.NET MVC (Razor), Duende IdentityServer, gRPC, SignalR, MediatR, Angular 16, nginx, Bootstrap
- SOFTWARE PLATFORMS: Visual Studio 2022, Visual Studio Code, SQL Server Management Studio (SSMS), GitHub, Git Extensions, SEQ, RabbitMQ, Docker, Docker-Compose


SPA UI позволяет (общие моменты):

- входить в систему и выходить из неё, на основании введённых пользователем данных, без обращения к внешним провайдерам (нужна доработка)
- регистрировать новых пользователей с подтверждением по email
- сбрасывать пароль, с отправкой токена сброса пароля на почту пользователя
- блокировать/разблокировать пользователя
- получать списки сущностей и фильтровать их
- создавать, обновлять, удалять заметки, поступления, перемещения, списания с реализованной на сервере логикой проверок допустимых действий
- выполнять массовое удаление документов движения товара
- валидацию данных
- обновлять настройки уведомлений для каждого пользователя (включенные уведомления отображаются вверху списка)
- получать обновленные данные в режиме онлайн (signalR) в разрезе поступлений, перемещений, списаний, в случаи, если данные были изменены другими пользователями
- вводить данные пользователя в личном кабинете
- загружать фотографию для личного кабинета
- загружать/скачивать документы пользователя в личном кабинете
- закреплять важные заметки в верхней части списка заметок
- обновлять сортировку заметок, путём их перетаскивания в рамках групп "закрепленные" и "незакрепленные"
- логировать ошибки и основные действия
- отменять действия удаления и редактирования в режиме онлайн
- выдавать пользователю уведомления через toastr о совершённых действиях,
- сохранять выбранные элементы при смене страницы и сортировки в разрезе колонок таблиц UI
- отслеживать в режиме онлайн работоспособность сервисов через встроенный UI


Тк приложение "носит" демонстрационный характер, то :

- в одном документе движения товара (поступление, списание, перемещение) указывается только одна позиция/номенклатура
- в документах перемещение и списание обновление/редактирование возможно только в части номера, цены и количества
- у каждой номенклатуры всегда уникальная партия - достигается за счет автоматически сгенерированного уникального номера склада хранения,
- на основании уникальности партии и строится логика приложения
- отсутсвуют некоторые проверки и фильтры при создании перемещений и списаний (например партии выбираются на текущий момент, а не исходя из даты создания
документа списания/перемещения, но реализована проверка даты и времени в зависимости от даты поступления используемой партии
- при создании перемещения/списания выпадающие списки будут формироваться на основании остатков на складе на текущий момент, исходя из
выбранных пользователем данных в форме создания)
- тесты в основном функциональные, модульные сделаны для демонстрации точечно, в основном в Note, EventBus, StockControlSPA
- workflows тоже сделаны только для некоторых микросервисов