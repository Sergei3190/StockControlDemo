#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ApiGateways/Web.Bff.StockControl/Web.StockControl.HttpAggregator/Web.StockControl.HttpAggregator.csproj", "src/ApiGateways/Web.Bff.StockControl/Web.StockControl.HttpAggregator/"]
COPY ["src/Services/Common/Service.Common/Service.Common.csproj", "src/Services/Common/Service.Common/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/BuildingBlocks/EventBus/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/EventBus/"]
COPY ["src/BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/BuildingBlocks/EventBus/IntegrationEventLogEF/"]
COPY ["src/Services/Common/Email.Service/Email.Service.csproj", "src/Services/Common/Email.Service/"]
COPY ["src/Services/Common/FileParser.Service/FileParser.Service.csproj", "src/Services/Common/FileParser.Service/"]
RUN dotnet restore "src/ApiGateways/Web.Bff.StockControl/Web.StockControl.HttpAggregator/Web.StockControl.HttpAggregator.csproj"
COPY . .
WORKDIR "/src/src/ApiGateways/Web.Bff.StockControl/Web.StockControl.HttpAggregator"
RUN dotnet build "Web.StockControl.HttpAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.StockControl.HttpAggregator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.StockControl.HttpAggregator.dll"]