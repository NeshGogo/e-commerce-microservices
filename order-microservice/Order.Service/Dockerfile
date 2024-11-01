FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release
COPY ["order-microservice/Order.Service/Order.Service.csproj", "Order.Service/"]
COPY ["local-nuget-packages/", "/local-nuget-packages/"]
COPY ["order-microservice/nuget.config", "./"]
run dotnet restore "Order.Service/Order.Service.csproj"

COPY ["order-microservice/Order.Service/", "Order.Service/"]
WORKDIR "Order.Service"
run dotnet build "Order.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
run dotnet publish "Order.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM publish as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Service.dll"]