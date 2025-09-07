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
COPY ["Hotelio.Booking.Service/Hotelio.Booking.Service.csproj", "Hotelio.Booking.Service/"]
COPY ["Hotelio.Booking.Migrations/Hotelio.Booking.Migrations.csproj", "Hotelio.Booking.Migrations/"]
COPY ["Hotelio.Booking.IntegrationEvents/Hotelio.Booking.IntegrationEvents.csproj", "Hotelio.Booking.IntegrationEvents/"]

RUN dotnet restore "Hotelio.Booking.IntegrationEvents/Hotelio.Booking.IntegrationEvents.csproj"
RUN dotnet restore "Hotelio.ServiceDefaults/Hotelio.ServiceDefaults.csproj"
RUN dotnet restore "Hotelio.Booking.Service/Hotelio.Booking.Service.csproj"
RUN dotnet restore "Hotelio.Booking.Migrations/Hotelio.Booking.Migrations.csproj"

COPY . .

RUN dotnet build "Hotelio.Booking.Migrations/Hotelio.Booking.Migrations.csproj" -c $BUILD_CONFIGURATION --no-restore

RUN dotnet ef migrations bundle \
    --no-build \
    --output /app/efbundle_booking.sh \
    --target-runtime linux-x64 \
    --project Hotelio.Booking.Migrations/Hotelio.Booking.Migrations.csproj \
    --startup-project Hotelio.Booking.Migrations/Hotelio.Booking.Migrations.csproj \
    --context Hotelio.Booking.Service.Infrastructure.BookingContext \
    --configuration $BUILD_CONFIGURATION \
    --framework net9.0

RUN chmod +x /app/efbundle_booking.sh

FROM base AS final
WORKDIR /app

COPY --from=build /app/efbundle_booking.sh .

ENTRYPOINT ["/bin/bash", "-c", "./efbundle_booking.sh"]
