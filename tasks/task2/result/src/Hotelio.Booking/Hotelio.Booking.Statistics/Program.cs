using Confluent.Kafka;
using Hotelio.Booking.Statistics;
using Hotelio.Booking.Statistics.Api;
using Hotelio.Booking.Statistics.EventHandlers;
using Hotelio.Booking.Statistics.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var withApiVersioning = builder.Services.AddApiVersioning();

builder
    .AddServiceDefaults()
    .AddDefaultOpenApi(withApiVersioning)
    ;

builder.Services.AddDbContext<BookingStatisticsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("booking_statistics_db")));
builder.EnrichNpgsqlDbContext<BookingStatisticsContext>();

builder.AddKafkaConsumer<string, string>("kafka", settings =>
{
    settings.Config.EnableAutoCommit = false;
    settings.Config.AutoOffsetReset = AutoOffsetReset.Earliest;
    settings.Config.GroupId = "booking_events";
});
builder.Services.AddScoped<IBookingCreatedEventHandler, BookingCreatedEventHandler>();
builder.Services.AddHostedService<BookingEventsWorker>();

var app = builder.Build();

app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseDefaultOpenApi();
}

app.MapStatisticsApi();

app.Run();