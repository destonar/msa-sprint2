FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ENV DOTNET_CLI_HOME=/tmp
WORKDIR /src

RUN dotnet tool install dotnet-ef --tool-path /tools --version 9.0
ENV PATH="$PATH:/tools"

COPY ["Hotelio.ServiceDefaults/Hotelio.ServiceDefaults.csproj", "Hotelio.ServiceDefaults/"]
COPY ["Hotelio.Booking.Statistics/Hotelio.Booking.Statistics.csproj", "Hotelio.Booking.Statistics/"]
COPY ["Hotelio.Booking.Statistics.Migrations/Hotelio.Booking.Statistics.Migrations.csproj", "Hotelio.Booking.Statistics.Migrations/"]
COPY ["Hotelio.Booking.IntegrationEvents/Hotelio.Booking.IntegrationEvents.csproj", "Hotelio.Booking.IntegrationEvents/"]

RUN dotnet restore "Hotelio.Booking.IntegrationEvents/Hotelio.Booking.IntegrationEvents.csproj"
RUN dotnet restore "Hotelio.ServiceDefaults/Hotelio.ServiceDefaults.csproj"
RUN dotnet restore "Hotelio.Booking.Statistics/Hotelio.Booking.Statistics.csproj"
RUN dotnet restore "Hotelio.Booking.Statistics.Migrations/Hotelio.Booking.Statistics.Migrations.csproj"

COPY . .

RUN dotnet build "./Hotelio.Booking.Statistics.Migrations/Hotelio.Booking.Statistics.Migrations.csproj" -c $BUILD_CONFIGURATION --no-restore

RUN dotnet ef migrations bundle \
    --no-build \
    --output /app/efbundle_booking_statistics.sh \
    --target-runtime linux-x64 \
    --project Hotelio.Booking.Statistics.Migrations/Hotelio.Booking.Statistics.Migrations.csproj \
    --startup-project Hotelio.Booking.Statistics.Migrations/Hotelio.Booking.Statistics.Migrations.csproj \
    --context Hotelio.Booking.Statistics.Infrastructure.BookingStatisticsContext \
    --configuration $BUILD_CONFIGURATION \
    --framework net9.0

RUN chmod +x /app/efbundle_booking_statistics.sh

FROM base AS final
WORKDIR /app

COPY --from=build /app/efbundle_booking_statistics.sh .

ENTRYPOINT ["/bin/bash", "-c", "./efbundle_booking_statistics.sh"]
