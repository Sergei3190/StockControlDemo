#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Identity/Identity.API.BackgroundTasks/Identity.API.BackgroundTasks.csproj", "src/Services/Identity/Identity.API.BackgroundTasks/"]
COPY ["src/Services/Common/Service.Common/Service.Common.csproj", "src/Services/Common/Service.Common/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/BuildingBlocks/EventBus/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/EventBus/"]
COPY ["src/BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/BuildingBlocks/EventBus/IntegrationEventLogEF/"]
COPY ["src/Services/Common/Email.Service/Email.Service.csproj", "src/Services/Common/Email.Service/"]
COPY ["src/Services/Common/FileParser.Service/FileParser.Service.csproj", "src/Services/Common/FileParser.Service/"]
COPY ["src/Services/Identity/Identity.API.DAL/Identity.API.DAL.csproj", "src/Services/Identity/Identity.API.DAL/"]
COPY ["src/Services/Identity/Identity.API.Domain/Identity.API.Domain.csproj", "src/Services/Identity/Identity.API.Domain/"]
RUN dotnet restore "src/Services/Identity/Identity.API.BackgroundTasks/Identity.API.BackgroundTasks.csproj"
COPY . .
WORKDIR "/src/src/Services/Identity/Identity.API.BackgroundTasks"
RUN dotnet build "Identity.API.BackgroundTasks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.API.BackgroundTasks.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.BackgroundTasks.dll"]