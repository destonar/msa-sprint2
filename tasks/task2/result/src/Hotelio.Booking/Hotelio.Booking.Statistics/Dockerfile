FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Hotelio.ServiceDefaults/Hotelio.ServiceDefaults.csproj", "Hotelio.ServiceDefaults/"]
COPY ["Hotelio.Booking.Statistics/Hotelio.Booking.Statistics.csproj", "Hotelio.Booking.Statistics/"]
COPY ["Hotelio.Booking.IntegrationEvents/Hotelio.Booking.IntegrationEvents.csproj", "Hotelio.Booking.IntegrationEvents/"]

RUN dotnet restore "Hotelio.Booking.IntegrationEvents/Hotelio.Booking.IntegrationEvents.csproj"
RUN dotnet restore "Hotelio.ServiceDefaults/Hotelio.ServiceDefaults.csproj"
RUN dotnet restore "Hotelio.Booking.Statistics/Hotelio.Booking.Statistics.csproj"
COPY . .
WORKDIR "/src/Hotelio.Booking.Statistics"
RUN dotnet build "./Hotelio.Booking.Statistics.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hotelio.Booking.Statistics.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotelio.Booking.Statistics.dll"]
