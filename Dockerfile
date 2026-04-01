# 1. Etapa de compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar archivos de proyecto y restaurar dependencias
COPY ["ProductsApi.slnx", "./"]
COPY ["src/api/ProductsApi.csproj", "src/api/"]
RUN dotnet restore "src/api/ProductsApi.csproj"

# Copiar el resto del código y compilar
COPY . .
WORKDIR "/app/src/api"
RUN dotnet publish "ProductsApi.csproj" -c Release -o /app/publish

# 2. Etapa de ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ProductsApi.dll"]