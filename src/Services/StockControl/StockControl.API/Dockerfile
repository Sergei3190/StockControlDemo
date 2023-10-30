#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/StockControl/StockControl.API/StockControl.API.csproj", "src/Services/StockControl/StockControl.API/"]
COPY ["src/Services/Common/Service.Common/Service.Common.csproj", "src/Services/Common/Service.Common/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/BuildingBlocks/EventBus/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/EventBus/"]
COPY ["src/BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/BuildingBlocks/EventBus/IntegrationEventLogEF/"]
COPY ["src/Services/Common/Email.Service/Email.Service.csproj", "src/Services/Common/Email.Service/"]
COPY ["src/Services/Common/FileParser.Service/FileParser.Service.csproj", "src/Services/Common/FileParser.Service/"]
COPY ["src/Services/StockControl/StockControl.API.DAL/StockControl.API.DAL.csproj", "src/Services/StockControl/StockControl.API.DAL/"]
COPY ["src/Services/StockControl/StockControl.API.Domain/StockControl.API.Domain.csproj", "src/Services/StockControl/StockControl.API.Domain/"]
COPY ["src/Services/StockControl/StockControl.API.Models/StockControl.API.Models.csproj", "src/Services/StockControl/StockControl.API.Models/"]
RUN dotnet restore "src/Services/StockControl/StockControl.API/StockControl.API.csproj"
COPY . .
WORKDIR "/src/src/Services/StockControl/StockControl.API"
RUN dotnet build "StockControl.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockControl.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockControl.API.dll"]