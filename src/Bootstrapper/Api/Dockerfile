# Etapa base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa para instalar vsdbg (depurador) - se puede usar para debug y fast mode
FROM base AS vsdbg
RUN apt-get update && apt-get install -y unzip curl \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg \
    && chmod +x /vsdbg/vsdbg
USER $APP_UID

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar todos los csproj (una sola vez)
COPY ["Bootstrapper/Api/Api.csproj", "Bootstrapper/Api/"]
COPY ["Modules/Carrito/Carrito/Carrito.csproj", "Modules/Carrito/Carrito/"]
COPY ["Modules/Catalogo/Catalogo/Catalogo.csproj", "Modules/Catalogo/Catalogo/"]
COPY ["Modules/Catalogo/Catalogo.Contracts/Catalogo.Contracts.csproj", "Modules/Catalogo/Catalogo.Contracts/"]
COPY ["Modules/Ordering/Ordering/Ordering.csproj", "Modules/Ordering/Ordering/"]
COPY ["Shared/Shared/Shared.csproj", "Shared/Shared/"]
COPY ["Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]
COPY ["Shared/Shared.Messaging/Shared.Messaging.csproj", "Shared/Shared.Messaging/"]

# Restaurar dependencias
RUN dotnet restore "Bootstrapper/Api/Api.csproj"

# Copiar el resto del código
COPY . .

# Build del proyecto
WORKDIR "/src/Bootstrapper/Api"
RUN dotnet build "Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa debug (reutiliza build + depurador)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS debug
WORKDIR /src

# Reutiliza instalación del depurador
RUN apt-get update && apt-get install -y unzip curl \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg \
    && chmod +x /vsdbg/vsdbg

# Copiar todo (una vez)
COPY . .

WORKDIR "/src/Bootstrapper/Api"
RUN dotnet build "Api.csproj" -c Debug -o /app/build
WORKDIR /app/build
EXPOSE 8080
ENTRYPOINT ["dotnet", "Api.dll"]

# Etapa final (producción)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]