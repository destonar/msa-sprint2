FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN echo "deb https://deb.debian.org/debian bookworm main\n\
deb https://deb.debian.org/debian bookworm-updates main\n\
deb https://deb.debian.org/debian-security bookworm-security main" > /etc/apt/sources.list \
    && apt-get update && apt-get install -y curl
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 5000
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Hotelio.ServiceDefaults/Hotelio.ServiceDefaults.csproj", "Hotelio.ServiceDefaults/"]
COPY ["Hotelio.Booking.Service/Hotelio.Booking.Service.csproj", "Hotelio.Booking.Service/"]
COPY ["Hotelio.Booking.IntegrationEvents/Hotelio.Booking.IntegrationEvents.csproj", "Hotelio.Booking.IntegrationEvents/"]

RUN dotnet restore "Hotelio.Booking.IntegrationEvents/Hotelio.Booking.IntegrationEvents.csproj"
RUN dotnet restore "Hotelio.ServiceDefaults/Hotelio.ServiceDefaults.csproj"
RUN dotnet restore "Hotelio.Booking.Service/Hotelio.Booking.Service.csproj"
COPY . .
WORKDIR "/src/Hotelio.Booking.Service"
RUN dotnet build "./Hotelio.Booking.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hotelio.Booking.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotelio.Booking.Service.dll"]
